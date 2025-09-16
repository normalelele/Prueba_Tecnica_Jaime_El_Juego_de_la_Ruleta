<template>

  <div class="game-results" v-if="lastResult">

    <div class="result-header">
      <h3>Resultado de la Ruleta</h3>
    </div>

    <div class="result-content">
      <div class="result-main">
        <div class="result-number" :class="lastResult.color">
          {{ lastResult.number }}
        </div>
        <div class="result-color" :class="lastResult.color">
          {{ colorToSpanish(lastResult.color).toUpperCase() }}
        </div>
        <div class="result-even-odd">
          {{ lastResult.number === 0 ? 'CERO' : (lastResult.number % 2 === 0 ? 'PAR' : 'IMPAR') }}
        </div>
      </div>

      <div class="bet-result" v-if="currentBet">
        <div class="bet-info">
          <h4>Tu Apuesta:</h4>
          <div class="bet-details">
            <p><strong>Tipo:</strong> {{ getBetTypeDescription() }}</p>
            <p><strong>Monto:</strong> ${{ currentBet.amount }}</p>
            <p><strong>Apuesta:</strong> {{ getBetValueDescription() }}</p>
          </div>
        </div>

        <div class="winnings" :class="winnings > 0 ? 'win' : 'loss'">
          <div class="winnings-label">
            {{ winnings > 0 ? '¡GANASTE!' : 'PERDISTE' }}
          </div>
          <div class="winnings-amount">
            {{ winnings > 0 ? '+' : '' }}${{ winnings.toFixed(2) }}
          </div>
        </div>
      </div>
    </div>

  </div>

</template>

<script setup lang="ts">

import type { GameResult, Bet } from '../types/Game';
import { colorToSpanish, parityToSpanish } from '../utils/translate';

interface Props {
  lastResult: GameResult | null;
  currentBet: Bet | null;
  winnings: number;
}

const props = defineProps<Props>();

const getBetTypeDescription = () => {
  if (!props.currentBet) return '';

  switch (props.currentBet.type) {
    case 'color':
      return 'Color';
    case 'even-odd-color':
      return 'Par/Impar de Color';
    case 'number-color':
      return 'Número y Color específicos';
    default:
      return 'Desconocido';
  }
};

const getBetValueDescription = () => {
  if (!props.currentBet) return '';

  switch (props.currentBet.type) {
    case 'color':
      return colorToSpanish(props.currentBet.value?.toString()).toUpperCase();

    case 'even-odd-color':
      const parityText = parityToSpanish(props.currentBet.value?.toString()).toUpperCase();
      const colorText = colorToSpanish(props.currentBet.color).toUpperCase();
      return `${parityText} ${colorText}`;

    case 'number-color':
      return `${props.currentBet.numberValue} ${colorToSpanish(props.currentBet.color).toUpperCase()}`;

    default:
      return 'N/A';
  }
};

</script>

<style scoped>
.game-results {
  background: linear-gradient(135deg, #1a1a1a, #2d2d2d);
  border: 2px solid #DAA520;
  border-radius: 12px;
  padding: 1.5rem;
  margin: 1rem 0;
  color: white;
  animation: slideIn 0.5s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.result-header h3 {
  color: #DAA520;
  text-align: center;
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
  font-weight: bold;
}

.result-content {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.result-main {
  text-align: center;
  padding: 1.5rem;
  background: rgba(0, 0, 0, 0.3);
  border-radius: 8px;
  border: 1px solid #4a4a4a;
}

.result-number {
  font-size: 4rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
}

.result-number.red {
  color: #dc2626;
}

.result-number.black {
  color: #9ca3af;
}

.result-number.green {
  color: #16a34a;
}

.result-color {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.result-color.red {
  color: #dc2626;
}

.result-color.black {
  color: #9ca3af;
}

.result-color.green {
  color: #16a34a;
}

.result-even-odd {
  font-size: 1rem;
  color: #9ca3af;
  font-weight: 600;
}

.bet-result {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  padding: 1.5rem;
  background: rgba(0, 0, 0, 0.3);
  border-radius: 8px;
  border: 1px solid #4a4a4a;
}

.bet-info h4 {
  color: #DAA520;
  margin-bottom: 1rem;
  font-size: 1.2rem;
}

.bet-details p {
  margin-bottom: 0.5rem;
  color: #e5e5e5;
}

.bet-details strong {
  color: #DAA520;
}

.winnings {
  text-align: center;
  padding: 1rem;
  border-radius: 8px;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.winnings.win {
  background: rgba(22, 163, 74, 0.2);
  border: 2px solid #16a34a;
}

.winnings.loss {
  background: rgba(220, 38, 38, 0.2);
  border: 2px solid #dc2626;
}

.winnings-label {
  font-size: 1.2rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.winnings.win .winnings-label {
  color: #16a34a;
}

.winnings.loss .winnings-label {
  color: #dc2626;
}

.winnings-amount {
  font-size: 2rem;
  font-weight: bold;
}

.winnings.win .winnings-amount {
  color: #16a34a;
}

.winnings.loss .winnings-amount {
  color: #dc2626;
}

@media (max-width: 768px) {
  .bet-result {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .result-number {
    font-size: 3rem;
  }

  .result-color {
    font-size: 1.2rem;
  }
}
</style>