<template>

  <div class="betting-panel">
    <h3 class="panel-title">Realizar Apuesta</h3>

    <div class="bet-amount">
      <label for="betAmount">Monto a apostar:</label>
      <input id="betAmount" v-model.number="betAmount" type="number" min="1" :max="maxBet" class="amount-input"
        :disabled="!canBet" />
    </div>

    <div class="bet-types">
      <div class="bet-type">
        <h4>Apostar por Color</h4>
        <p class="bet-info">Gana la mitad de tu apuesta si aciertas</p>
        <div class="color-buttons">
          <button @click="placeBet('color', 'red')" class="bet-button color-red" :disabled="!canBet || betAmount <= 0">
            ROJO
          </button>
          <button @click="placeBet('color', 'black')" class="bet-button color-black"
            :disabled="!canBet || betAmount <= 0">
            NEGRO
          </button>
        </div>
      </div>

      <div class="bet-type">
        <h4>Par/Impar de un Color</h4>
        <p class="bet-info">Gana tu apuesta completa si aciertas</p>
        <div class="even-odd-section">
          <div class="color-group">
            <span class="color-label red">ROJO:</span>
            <button @click="placeBet('even-odd-color', 'even', 'red')" class="bet-button small"
              :disabled="!canBet || betAmount <= 0">
              PAR
            </button>
            <button @click="placeBet('even-odd-color', 'odd', 'red')" class="bet-button small"
              :disabled="!canBet || betAmount <= 0">
              IMPAR
            </button>
          </div>
          <div class="color-group">
            <span class="color-label black">NEGRO:</span>
            <button @click="placeBet('even-odd-color', 'even', 'black')" class="bet-button small"
              :disabled="!canBet || betAmount <= 0">
              PAR
            </button>
            <button @click="placeBet('even-odd-color', 'odd', 'black')" class="bet-button small"
              :disabled="!canBet || betAmount <= 0">
              IMPAR
            </button>
          </div>
        </div>
      </div>
      
      <div class="bet-type">
        <h4>Número y Color Específico</h4>
        <p class="bet-info">Gana el triple de tu apuesta si aciertas</p>
        <div class="number-color-section">
          <div class="number-input">
            <label for="specificNumber">Número (0-36):</label>
            <input id="specificNumber" v-model.number="specificNumber" type="number" min="0" max="36"
              class="number-field" />
          </div>
          <div class="specific-color-buttons">
            <button @click="placeBet('number-color', specificNumber, 'red')" class="bet-button color-red"
              :disabled="!canBet || betAmount <= 0 || specificNumber < 0 || specificNumber > 36">
              {{ specificNumber }} ROJO
            </button>
            <button @click="placeBet('number-color', specificNumber, 'black')" class="bet-button color-black"
              :disabled="!canBet || betAmount <= 0 || specificNumber < 0 || specificNumber > 36">
              {{ specificNumber }} NEGRO
            </button>
            <button v-if="specificNumber === 0" @click="placeBet('number-color', specificNumber, 'green')"
              class="bet-button color-green" :disabled="!canBet || betAmount <= 0">
              0 VERDE
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>

</template>

<script setup lang="ts">

import { computed, ref } from 'vue';
import type { Bet } from '../types/Game';

interface Props {
  userBalance: number;
  canBet: boolean;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  placeBet: [bet: Bet];
}>();

const betAmount = ref(10);
const specificNumber = ref(7);

const maxBet = computed(() => props.userBalance);

const placeBet = (type: string, value: string | number, color?: string) => {
  if (betAmount.value <= 0 || betAmount.value > props.userBalance) return;

  const bet: Bet = {
    type: type as 'color' | 'even-odd-color' | 'number-color',
    amount: betAmount.value,
    value: value,
    color: color as 'red' | 'black',
    numberValue: type === 'number-color' ? specificNumber.value : undefined
  };

  emit('placeBet', bet);
};

</script>

<style scoped>
.betting-panel {
  background: linear-gradient(135deg, #1a1a1a, #2d2d2d);
  border: 2px solid #DAA520;
  border-radius: 12px;
  padding: 1.5rem;
  margin: 1rem 0;
  color: white;
}

.panel-title {
  color: #DAA520;
  margin-bottom: 1.5rem;
  text-align: center;
  font-size: 1.5rem;
  font-weight: bold;
}

.bet-amount {
  margin-bottom: 2rem;
}

.bet-amount label {
  display: block;
  margin-bottom: 0.5rem;
  color: #DAA520;
  font-weight: 600;
}

.amount-input {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #4a4a4a;
  border-radius: 6px;
  background: #333;
  color: white;
  font-size: 1.1rem;
  text-align: center;
}

.amount-input:focus {
  outline: none;
  border-color: #DAA520;
}

.bet-types {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.bet-type {
  border: 1px solid #4a4a4a;
  border-radius: 8px;
  padding: 1.5rem;
  background: rgba(0, 0, 0, 0.3);
}

.bet-type h4 {
  color: #DAA520;
  margin-bottom: 0.5rem;
  font-size: 1.2rem;
}

.bet-info {
  color: #9ca3af;
  font-size: 0.875rem;
  margin-bottom: 1rem;
  font-style: italic;
}

.color-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

.bet-button {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 1rem;
  min-width: 100px;
}

.bet-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
}

.bet-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.color-red {
  background: #dc2626;
  color: white;
}

.color-black {
  background: #1f2937;
  color: white;
}

.color-green {
  background: #16a34a;
  color: white;
}

.bet-button.small {
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
  min-width: 60px;
}

.even-odd-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.color-group {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.color-label {
  font-weight: bold;
  min-width: 60px;
}

.color-label.red {
  color: #dc2626;
}

.color-label.black {
  color: #9ca3af;
}

.number-color-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.number-input label {
  color: #DAA520;
  margin-bottom: 0.5rem;
  display: block;
}

.number-field {
  padding: 0.5rem;
  border: 1px solid #4a4a4a;
  border-radius: 4px;
  background: #333;
  color: white;
  width: 100px;
}

.specific-color-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

@media (max-width: 768px) {

  .color-buttons,
  .specific-color-buttons {
    flex-direction: column;
  }

  .color-group {
    flex-direction: column;
    align-items: flex-start;
  }

  .even-odd-section {
    gap: 1.5rem;
  }
}
</style>