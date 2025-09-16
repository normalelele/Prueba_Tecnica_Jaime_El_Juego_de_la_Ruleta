<template>

  <div class="roulette-container">

    <div class="roulette-wheel" :class="{ spinning: isSpinning }">
      <div class="outer-ring"></div>

      <div class="numbers-ring">
        <div v-for="(number, index) in rouletteNumbers" :key="index" class="number-slot"
          :class="[getNumberColor(number.value), { 'winning-number': lastResult && lastResult.number === number.value }]"
          :style="{ transform: `rotate(${index * (360 / rouletteNumbers.length)}deg)` }">
          <span class="number-text">{{ number.value }}</span>
        </div>
      </div>

      <div class="center-hub">
        <div class="center-circle"></div>
      </div>

      <div class="indicator"></div>
    </div>

    <div v-if="lastResult" class="current-result">
      <div class="result-display" :class="lastResult.color">
        <span class="result-number">{{ lastResult.number }}</span>
        <span class="result-color">{{ colorToSpanish(lastResult.color).toUpperCase() }}</span>
      </div>
    </div>

  </div>

</template>

<script setup lang="ts">

import { colorToSpanish } from '../utils/translate';
import { ref } from 'vue';
import type { GameResult } from '../types/Game';

interface Props {
  isSpinning: boolean;
  lastResult: GameResult | null;
}

const props = defineProps<Props>();

const rouletteNumbers = ref([
  { value: 0 }, { value: 1 }, { value: 2 }, { value: 3 }, { value: 4 }, { value: 5 },
  { value: 6 }, { value: 7 }, { value: 8 }, { value: 9 }, { value: 10 }, { value: 11 },
  { value: 12 }, { value: 13 }, { value: 14 }, { value: 15 }, { value: 16 }, { value: 17 },
  { value: 18 }, { value: 19 }, { value: 20 }, { value: 21 }, { value: 22 }, { value: 23 },
  { value: 24 }, { value: 25 }, { value: 26 }, { value: 27 }, { value: 28 }, { value: 29 },
  { value: 30 }, { value: 31 }, { value: 32 }, { value: 33 }, { value: 34 }, { value: 35 },
  { value: 36 }
]);

const getNumberColor = (numberValue: number) => {
  if (!props.lastResult) return 'neutral';

  if (props.lastResult.number === numberValue) {
    return props.lastResult.color;
  }

  return 'neutral';
};

</script>

<style scoped>
.roulette-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  padding: 2rem;
}

.roulette-wheel {
  position: relative;
  width: 400px;
  height: 400px;
  border-radius: 50%;
  background: linear-gradient(45deg, #8B4513, #A0522D);
  border: 8px solid #DAA520;
  box-shadow: 0 0 30px rgba(218, 165, 32, 0.5), inset 0 0 20px rgba(0, 0, 0, 0.3);
  transition: transform 3s cubic-bezier(0.23, 1, 0.32, 1);
}

.roulette-wheel.spinning {
  animation: spin 3s cubic-bezier(0.23, 1, 0.32, 1) forwards;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(1800deg);
  }
}

.outer-ring {
  position: absolute;
  top: 17px;
  left: 10px;
  right: 10px;
  bottom: 10px;
  border-radius: 50%;
  background: linear-gradient(45deg, #2d2d2d, #1a1a1a);
  border: 3px solid #DAA520;
}

.numbers-ring {
  position: absolute;
  top: 20px;
  left: 20px;
  right: 20px;
  bottom: 20px;
  border-radius: 50%;
}

.number-slot {
  position: absolute;
  top: 0;
  left: 50%;
  width: 30px;
  height: 60px;
  margin-left: -15px;
  transform-origin: 50% 180px;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding-top: 5px;
}

.number-slot.red {
  background: linear-gradient(180deg, #dc2626, #b91c1c);
}

.number-slot.black {
  background: linear-gradient(180deg, #374151, #1f2937);
}

.number-slot.neutral {
  background: linear-gradient(180deg, #6b7280, #4b5563);
}

.number-slot.winning-number {
  background: linear-gradient(180deg, #fbbf24, #f59e0b) !important;
  box-shadow: 0 0 15px #fbbf24;
  animation: pulse 1s infinite;
}

@keyframes pulse {

  0%,
  100% {
    box-shadow: 0 0 15px #fbbf24;
  }

  50% {
    box-shadow: 0 0 25px #fbbf24, 0 0 35px #fbbf24;
  }
}

.number-text {
  color: white;
  font-weight: bold;
  font-size: 0.875rem;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.8);
  transform: rotate(180deg);
}

.center-hub {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background: linear-gradient(45deg, #DAA520, #B8860B);
  border: 3px solid #8B4513;
  box-shadow: 0 0 20px rgba(218, 165, 32, 0.6), inset 0 0 10px rgba(0, 0, 0, 0.3);
}

.center-circle {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: linear-gradient(45deg, #1a1a1a, #2d2d2d);
  border: 2px solid #DAA520;
}

.indicator {
  position: absolute;
  top: -15px;
  left: 50%;
  transform: translateX(-50%);
  width: 0;
  height: 0;
  border-left: 15px solid transparent;
  border-right: 15px solid transparent;
  border-top: 30px solid #DAA520;
  z-index: 10;
  filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
}

.current-result {
  text-align: center;
}

.result-display {
  display: inline-flex;
  flex-direction: column;
  align-items: center;
  padding: 1rem 2rem;
  border-radius: 12px;
  border: 3px solid;
  background: rgba(0, 0, 0, 0.8);
  min-width: 120px;
}

.result-display.red {
  border-color: #dc2626;
  box-shadow: 0 0 20px rgba(220, 38, 38, 0.3);
}

.result-display.black {
  border-color: #9ca3af;
  box-shadow: 0 0 20px rgba(156, 163, 175, 0.3);
}

.result-number {
  font-size: 3rem;
  font-weight: bold;
  color: white;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.8);
}

.result-color {
  font-size: 1rem;
  font-weight: 600;
  color: #DAA520;
  margin-top: 0.5rem;
}

@media (max-width: 768px) {
  .roulette-wheel {
    width: 300px;
    height: 300px;
  }

  .number-slot {
    width: 25px;
    height: 45px;
    margin-left: -12.5px;
    transform-origin: 50% 135px;
  }

  .number-text {
    font-size: 0.75rem;
  }

  .center-hub {
    width: 60px;
    height: 60px;
  }

  .center-circle {
    width: 30px;
    height: 30px;
  }

  .result-number {
    font-size: 2rem;
  }
}

@media (max-width: 480px) {
  .roulette-container {
    padding: 1rem;
  }

  .roulette-wheel {
    width: 250px;
    height: 250px;
  }

  .number-slot {
    width: 20px;
    height: 35px;
    margin-left: -10px;
    transform-origin: 50% 115px;
  }

  .number-text {
    font-size: 0.625rem;
  }
}

</style>