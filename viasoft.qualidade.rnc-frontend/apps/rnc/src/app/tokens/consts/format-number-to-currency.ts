import { CurrencyPipe } from '@angular/common';

export function formatNumberToCurrency(currencyPipe: CurrencyPipe, value: number, decimal = 2): string {
  return currencyPipe.transform(value || 0, 'BRL', 'symbol', `1.2-${decimal}`);
}
