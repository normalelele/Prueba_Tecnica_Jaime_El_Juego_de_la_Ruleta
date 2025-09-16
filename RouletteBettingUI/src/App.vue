<template>

  <div id="app">
    <header class="game-header">
      <h1>🎰💵 JUEGO DE LA RULETA 🎰💵</h1>
      <p class="subtitle">¡Haz tu apuesta y gira la ruleta!</p>
    </header>

    <main class="game-content">

      <section class="user-section">
        <UserPanel :current-user="gameState.currentUser" @user-changed="handleUserChanged" @show-message="showMessage"
          ref="userPanelRef" />
      </section>

      <section v-if="gameState.currentUser" class="game-section">
        <div class="roulette-section">
          <RouletteWheel :is-spinning="gameState.isSpinning" :last-result="gameState.lastResult" />

          <div class="spin-controls">
            <button v-if="gameState.currentBet && !gameState.isSpinning" @click="spinRoulette" class="spin-button"
              :disabled="gameState.isSpinning">
              {{ gameState.isSpinning ? 'GIRANDO...' : 'GIRAR RULETA' }}
            </button>

            <div v-if="gameState.currentBet && !gameState.isSpinning" class="current-bet-display">
              <h4>Apuesta Actual:</h4>
              <p>{{ getCurrentBetDescription() }}</p>
              <p class="bet-amount">Monto: ${{ gameState.currentBet.amount }}</p>
            </div>
          </div>
        </div>

        <div v-if="gameState.lastResult" class="results-section">
          <GameResults :last-result="gameState.lastResult" :current-bet="gameState.currentBet"
            :winnings="lastWinnings" />
        </div>

        <div v-if="!gameState.isSpinning" class="betting-section">
          <BettingPanel :user-balance="gameState.currentUser.balance"
            :can-bet="!gameState.currentBet || !!gameState.lastResult" @place-bet="handlePlaceBet" />
        </div>
      </section>

      <section v-else class="welcome-section">
        <div class="welcome-card">
          <h2>¡El Juego de la Ruleta!</h2>
          <p>Para comenzar a jugar, inicia un nuevo juego con tu nombre y saldo inicial, o carga un jugador existente.
          </p>
          <div class="game-rules">
            <h3>Reglas del Juego:</h3>
            <ul>
              <li><strong>Color:</strong> Gana la mitad de tu apuesta si aciertas</li>
              <li><strong>Par/Impar de Color:</strong> Gana tu apuesta completa si aciertas</li>
              <li><strong>Número y Color específicos:</strong> Gana el triple de tu apuesta si aciertas</li>
            </ul>
          </div>
        </div>
      </section>

    </main>

    <div v-if="notification.message" class="notification" :class="notification.type">
      {{ notification.message }}
    </div>
  </div>

</template>

<script setup lang="ts">

import UserPanel from './components/UserPanel.vue';
import RouletteWheel from './components/RouletteWheel.vue';
import BettingPanel from './components/BettingPanel.vue';
import GameResults from './components/GameResults.vue';
import type { GameState, User, Bet } from './types/Game';
import { reactive, ref } from 'vue';
import ApiService from './services/ApiService';
import { colorToSpanish } from './utils/translate';

const gameState = reactive<GameState>({
  currentUser: null,
  currentBet: null,
  lastResult: null,
  isSpinning: false,
  gameStarted: false
});

const notification = reactive({
  message: '',
  type: 'info' as 'success' | 'error' | 'info'
});

const lastWinnings = ref(0);
const userPanelRef = ref();

const handleUserChanged = (user: User | null) => {
  gameState.currentUser = user;
  gameState.currentBet = null;
  gameState.lastResult = null;
  gameState.gameStarted = !!user;
  lastWinnings.value = 0;
};

const handlePlaceBet = (bet: Bet) => {
  if (!gameState.currentUser || bet.amount > gameState.currentUser.balance) {
    showMessage('Saldo insuficiente para esta apuesta', 'error');
    return;
  }

  gameState.currentBet = bet;
  gameState.lastResult = null;
  lastWinnings.value = 0;

  showMessage(`Apuesta realizada: $${bet.amount}`, 'info');
};

const spinRoulette = async () => {
  if (!gameState.currentBet || !gameState.currentUser) return;

  gameState.isSpinning = true;

  try {
    const result = await ApiService.getRouletteResult();

    const prizeResponse = await ApiService.calculatePrize({
      type: gameState.currentBet.type,
      color: gameState.currentBet.color,
      parity: gameState.currentBet.type === 'even-odd-color' ? (gameState.currentBet.value as string) : undefined,
      number: gameState.currentBet.type === 'number-color' ? (gameState.currentBet.numberValue as number) : undefined,
      betAmount: gameState.currentBet.amount,
      resultNumber: result.number,
      resultColor: result.color
    });

    const winnings = prizeResponse.prize ?? 0;

    gameState.currentUser.balance += winnings;

    gameState.lastResult = result;
    lastWinnings.value = winnings;

    if (winnings > 0) {
      showMessage(`¡Felicidades! Ganaste $${winnings.toFixed(2)}`, 'success');
    } else {
      showMessage(
        `Lo siento, perdiste $${gameState.currentBet.amount.toFixed(2)}`,
        'error'
      );
    }

    if (gameState.currentUser.balance <= 0) {
      setTimeout(() => {
        showMessage('Te quedaste sin saldo. ¡Gracias por jugar!', 'info');
        gameState.currentUser = null;
        gameState.gameStarted = false;
      }, 3000);
    }
  } catch (error) {
    showMessage('Error al obtener el resultado de la ruleta', 'error');
  } finally {
    gameState.isSpinning = false;
  }
};

const getCurrentBetDescription = () => {
  if (!gameState.currentBet) return '';

  switch (gameState.currentBet.type) {
    case 'color':
      return `Color: ${colorToSpanish(gameState.currentBet.value?.toString()).toUpperCase()}`;

    case 'even-odd-color':
      return `${gameState.currentBet.value.toString().toUpperCase()} ${colorToSpanish(gameState.currentBet.color).toUpperCase()}`;

    case 'number-color':
      return `${gameState.currentBet.numberValue} ${colorToSpanish(gameState.currentBet.color).toUpperCase()}`;

    default:
      return 'Apuesta desconocida';
  }
};

const showMessage = (message: string, type: 'success' | 'error' | 'info') => {
  notification.message = message;
  notification.type = type;

  setTimeout(() => {
    notification.message = '';
  }, 4000);
};

</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  background: linear-gradient(135deg, #0f172a, #1e293b, #334155);
  min-height: 100vh;
  color: #e2e8f0;
  line-height: 1.6;
}

#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.game-header {
  background: linear-gradient(135deg, #1a1a1a, #2d2d2d);
  border-bottom: 3px solid #DAA520;
  padding: 2rem 1rem;
  text-align: center;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
}

.game-header h1 {
  color: #DAA520;
  font-size: 2.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
}

.subtitle {
  color: #9ca3af;
  font-size: 1.1rem;
  font-style: italic;
}

.game-content {
  flex: 1;
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
  width: 100%;
}

.user-section {
  margin-bottom: 2rem;
}

.game-section {
  display: grid;
  grid-template-columns: 1fr;
  gap: 2rem;
}

.roulette-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
}

.spin-controls {
  text-align: center;
}

.spin-button {
  background: linear-gradient(135deg, #DAA520, #B8860B);
  color: white;
  border: none;
  padding: 1rem 2rem;
  font-size: 1.2rem;
  font-weight: bold;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  text-transform: uppercase;
  letter-spacing: 1px;
  box-shadow: 0 4px 15px rgba(218, 165, 32, 0.3);
}

.spin-button:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 6px 25px rgba(218, 165, 32, 0.4);
}

.spin-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.current-bet-display {
  margin-top: 1.5rem;
  padding: 1rem;
  background: rgba(0, 0, 0, 0.3);
  border: 1px solid #4a4a4a;
  border-radius: 8px;
  color: white;
}

.current-bet-display h4 {
  color: #DAA520;
  margin-bottom: 0.5rem;
}

.bet-amount {
  font-weight: bold;
  color: #16a34a;
}

.welcome-section {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 400px;
}

.welcome-card {
  background: linear-gradient(135deg, #1a1a1a, #2d2d2d);
  border: 2px solid #DAA520;
  border-radius: 12px;
  padding: 3rem;
  text-align: center;
  max-width: 600px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
}

.welcome-card h2 {
  color: #DAA520;
  font-size: 2rem;
  margin-bottom: 1rem;
}

.welcome-card>p {
  font-size: 1.1rem;
  margin-bottom: 2rem;
  color: #9ca3af;
}

.game-rules {
  text-align: left;
  margin-top: 2rem;
}

.game-rules h3 {
  color: #DAA520;
  margin-bottom: 1rem;
  text-align: center;
}

.game-rules ul {
  list-style: none;
  padding: 0;
}

.game-rules li {
  padding: 0.5rem 0;
  border-bottom: 1px solid #4a4a4a;
}

.game-rules li:last-child {
  border-bottom: none;
}

.notification {
  position: fixed;
  top: 20px;
  right: 20px;
  padding: 1rem 1.5rem;
  border-radius: 8px;
  font-weight: 600;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  z-index: 1000;
  animation: slideInRight 0.3s ease-out;
}

@keyframes slideInRight {
  from {
    transform: translateX(100%);
    opacity: 0;
  }

  to {
    transform: translateX(0);
    opacity: 1;
  }
}

.notification.success {
  background: rgba(22, 163, 74, 0.9);
  border: 1px solid #16a34a;
  color: white;
}

.notification.error {
  background: rgba(220, 38, 38, 0.9);
  border: 1px solid #dc2626;
  color: white;
}

.notification.info {
  background: rgba(37, 99, 235, 0.9);
  border: 1px solid #2563eb;
  color: white;
}

@media (max-width: 768px) {
  .game-header h1 {
    font-size: 2rem;
  }

  .subtitle {
    font-size: 1rem;
  }

  .game-content {
    padding: 1rem;
  }

  .welcome-card {
    padding: 2rem;
    margin: 1rem;
  }

  .notification {
    right: 10px;
    left: 10px;
    top: 10px;
  }
}

@media (min-width: 1024px) {
  .game-section {
    grid-template-columns: 1fr 1fr;
    gap: 3rem;
  }

  .roulette-section {
    grid-column: span 2;
  }
}
</style>