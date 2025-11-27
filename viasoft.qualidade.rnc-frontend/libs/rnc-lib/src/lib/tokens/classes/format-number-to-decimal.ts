import { DecimalPipe } from '@angular/common';

export function formatNumberToDecimal(decimalPipe: DecimalPipe, valor: number, decimal: number): string {
  return decimalPipe.transform(valor, `1.6-${decimal}`);
}
