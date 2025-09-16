export interface User {
  username: string;
  balance: number;
}

export interface GameResult {
  number: number;
  color: 'red' | 'black' | 'green';
  parity?: 'even' | 'odd' | 'none';
}

export type BetType = 'color' | 'even-odd-color' | 'number-color';

export interface Bet {
  type: BetType;
  amount: number;
  value: string | number;
  color?: 'red' | 'black' | 'green';
  numberValue?: number;
}

export interface GameState {
  currentUser: User | null;
  currentBet: Bet | null;
  lastResult: GameResult | null;
  isSpinning: boolean;
  gameStarted: boolean;
}

// ----- API -----
export interface ApiUserDto {
  username: string;
  balance: number;
}

export interface ApiSpinResultDto {
  number: number;
  color: 'red' | 'black' | 'green' | string;
  parity: 'even' | 'odd' | 'none' | string;
}

export interface ApiBetRequest {
  type: string;
  color?: string;
  parity?: string;
  number?: number;
  betAmount: number;
  resultNumber?: number;
  resultColor?: string;
}

export interface ApiBetResult {
  won: boolean;
  prize: number;
}