<template>

  <div class="user-panel">

    <div class="user-info" v-if="currentUser">
      <h2 class="user-name">{{ currentUser.username }}</h2>
      <div class="balance-display">
        <span class="balance-label">Saldo:</span>
        <span class="balance-amount">${{ currentUser.balance.toFixed(2) }}</span>
      </div>
    </div>

    <div class="user-actions">

      <div v-if="!currentUser" class="start-game-section">
        <h3>Iniciar Juego</h3>

        <div class="input-group">
          <label for="playerName">Nombre del jugador:</label>
          <input id="playerName" v-model="playerName" type="text" placeholder="Ingresa tu nombre" class="name-input" />
        </div>

        <div class="input-group">
          <label for="initialBalance">Saldo inicial:</label>
          <input id="initialBalance" v-model.number="initialBalance" type="number" min="1" placeholder="100"
            class="balance-input" />
        </div>

        <button @click="startNewGame" :disabled="!playerName.trim() || initialBalance <= 0"
          class="action-button start-button">Iniciar Juego</button>
      </div>

      <div v-if="!currentUser" class="load-game-section">
        <h3>Cargar Jugador</h3>

        <div class="input-group">
          <label for="loadPlayerName">Nombre del jugador:</label>
          <input id="loadPlayerName" v-model="loadPlayerName" type="text" placeholder="Nombre a cargar"
            class="name-input" />
        </div>

        <button @click="loadExistingPlayer" :disabled="!loadPlayerName.trim() || isLoading"
          class="action-button load-button">
          {{ isLoading ? 'Cargando...' : 'Cargar Jugador' }}
        </button>

      </div>

      <div v-if="currentUser" class="game-actions">
        <button @click="saveProgress" :disabled="isSaving" class="action-button save-button">
          {{ isSaving ? 'Guardando...' : 'Guardar Progreso' }}
        </button>
        <button @click="endGame" class="action-button end-button">Terminar Juego</button>
      </div>

    </div>

    <div v-if="message" class="message" :class="messageType">
      {{ message }}
    </div>

  </div>

</template>

<script setup lang="ts">

import { ref } from 'vue';
import type { User } from '../types/Game';
import ApiService from '../services/ApiService';

interface Props {
  currentUser: User | null;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  userChanged: [user: User | null];
  showMessage: [message: string, type: 'success' | 'error' | 'info'];
}>();

const playerName = ref('');
const initialBalance = ref(100);
const loadPlayerName = ref('');
const isLoading = ref(false);
const isSaving = ref(false);
const message = ref('');
const messageType = ref<'success' | 'error' | 'info'>('info');

const startNewGame = () => {
  if (!playerName.value.trim() || initialBalance.value <= 0) return;

  const newUser: User = {
    username: playerName.value.trim(),
    balance: initialBalance.value
  };

  emit('userChanged', newUser);
  emit('showMessage', `¡Bienvenido ${newUser.username}! Juego iniciado con $${newUser.balance}`, 'success');

  playerName.value = '';
  initialBalance.value = 100;
};

const loadExistingPlayer = async () => {
  if (!loadPlayerName.value.trim()) return;

  isLoading.value = true;
  try {
    const user = await ApiService.getUserBalance(loadPlayerName.value.trim());

    if (user) {
      emit('userChanged', user);
      emit('showMessage', `¡Bienvenido de vuelta ${user.username}! Saldo: $${user.balance}`, 'success');
    } else {
      emit('showMessage', `No se encontró el jugador "${loadPlayerName.value}"`, 'error');
    }
  } catch (error) {
    emit('showMessage', 'Error al cargar el jugador', 'error');
  } finally {
    isLoading.value = false;
    loadPlayerName.value = '';
  }
};

const saveProgress = async () => {
  if (!props.currentUser) return;

  isSaving.value = true;
  try {
    const success = await ApiService.saveUserBalance(
      props.currentUser.username,
      props.currentUser.balance
    );

    if (success) {
      emit('showMessage', 'Progreso guardado exitosamente', 'success');
    } else {
      emit('showMessage', 'Error al guardar el progreso', 'error');
    }
  } catch (error) {
    emit('showMessage', 'Error al guardar el progreso', 'error');
  } finally {
    isSaving.value = false;
  }
};

const endGame = () => {
  if (confirm('¿Estás seguro que deseas terminar el juego? Los datos no guardados se perderán.')) {
    emit('userChanged', null);
    emit('showMessage', 'Juego terminado. ¡Gracias por jugar!', 'info');
  }
};

const showMessage = (msg: string, type: 'success' | 'error' | 'info') => {
  message.value = msg;
  messageType.value = type;
  setTimeout(() => {
    message.value = '';
  }, 3000);
};

defineExpose({ showMessage });
</script>

<style scoped>
.user-panel {
  background: linear-gradient(135deg, #1a1a1a, #2d2d2d);
  border: 2px solid #DAA520;
  border-radius: 12px;
  padding: 1.5rem;
  color: white;
  margin-bottom: 1rem;
}

.user-info {
  text-align: center;
  margin-bottom: 1.5rem;
}

.user-name {
  color: #DAA520;
  font-size: 1.75rem;
  margin-bottom: 0.5rem;
  font-weight: bold;
}

.balance-display {
  font-size: 1.25rem;
}

.balance-label {
  color: #9ca3af;
  margin-right: 0.5rem;
}

.balance-amount {
  color: #16a34a;
  font-weight: bold;
  font-size: 1.5rem;
}

.user-actions {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.start-game-section,
.load-game-section {
  border: 1px solid #4a4a4a;
  border-radius: 8px;
  padding: 1.5rem;
  background: rgba(0, 0, 0, 0.3);
}

.start-game-section h3,
.load-game-section h3 {
  color: #DAA520;
  margin-bottom: 1rem;
  font-size: 1.2rem;
}

.input-group {
  margin-bottom: 1rem;
}

.input-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #DAA520;
  font-weight: 600;
}

.name-input,
.balance-input {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #4a4a4a;
  border-radius: 6px;
  background: #333;
  color: white;
  font-size: 1rem;
}

.name-input:focus,
.balance-input:focus {
  outline: none;
  border-color: #DAA520;
}

.action-button {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s ease;
  font-size: 1rem;
}

.action-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
}

.action-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.start-button {
  background: #16a34a;
  color: white;
}

.load-button {
  background: #2563eb;
  color: white;
}

.save-button {
  background: #ca8a04;
  color: white;
}

.end-button {
  background: #dc2626;
  color: white;
}

.game-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

.message {
  margin-top: 1rem;
  padding: 0.75rem;
  border-radius: 6px;
  font-weight: 600;
  text-align: center;
}

.message.success {
  background: rgba(22, 163, 74, 0.2);
  border: 1px solid #16a34a;
  color: #16a34a;
}

.message.error {
  background: rgba(220, 38, 38, 0.2);
  border: 1px solid #dc2626;
  color: #dc2626;
}

.message.info {
  background: rgba(37, 99, 235, 0.2);
  border: 1px solid #2563eb;
  color: #2563eb;
}

@media (max-width: 768px) {
  .game-actions {
    flex-direction: column;
  }

  .user-actions {
    gap: 1rem;
  }

  .start-game-section,
  .load-game-section {
    padding: 1rem;
  }
}
</style>