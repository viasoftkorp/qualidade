import { DecimalPipe } from '@angular/common';

export function formatNumberToDecimal(decimalPipe: DecimalPipe, valor: number, decimal = 2): string {
  return decimalPipe.transform(valor, `1.2-${decimal}`);
}
