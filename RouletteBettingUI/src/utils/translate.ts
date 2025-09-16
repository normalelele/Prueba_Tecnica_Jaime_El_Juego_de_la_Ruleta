export function colorToSpanish(color?: string | null): string {
  if (!color) return '';
  switch (color.toLowerCase()) {
    case 'red': return 'rojo';
    case 'black': return 'negro';
    case 'green': return 'verde';
    default: return color;
  }
}

export function parityToSpanish(parity?: string | null): string {
  if (!parity) return '';
  switch (parity.toLowerCase()) {
    case 'even': return 'par';
    case 'odd': return 'impar';
    case 'none': return 'ninguno';
    default: return parity;
  }
}
