export class UltraMath
{
  public static Sum<T>(arrayToSum: T[], getValue: (item: T) => number): number {
    return arrayToSum.reduce((total, item) => {
      return total + getValue(item);
    }, 0);
  }

  public static RoundABNT(num: number, digits = 2): number {
    if (num === 0 || !isFinite(num)) {
      return num;
    }

    const extraDigits = 10;
    const str = num.toFixed(digits + extraDigits);

    const parts = str.split('.');
    const integerPart = parts[0];
    const decimalPart = parts[1] || '';

    if (decimalPart.length <= digits) {
      return num;
    }

    const nextDigit = parseInt(decimalPart.charAt(digits), 10);
    const lastDigit = parseInt(decimalPart.charAt(digits - 1), 10);

    const remainingDigits = decimalPart.substring(digits + 1);
    const allZeros = remainingDigits.split('').every(d => d === '0');

    let roundedDecimal = decimalPart.substring(0, digits);

    if (nextDigit < 5) {
    } else if (nextDigit > 5 || (nextDigit === 5 && !allZeros)) {
      let decimalAsNumber = parseInt(roundedDecimal, 10) + 1;

      if (decimalAsNumber.toString().length > roundedDecimal.length) {
        const integerAsNumber = parseInt(integerPart, 10) + 1;
        return parseFloat(`${integerAsNumber}.${decimalAsNumber.toString().substring(1)}`);
      }

      roundedDecimal = decimalAsNumber.toString().padStart(digits, '0');
    } else if (nextDigit === 5 && allZeros) {
      if (lastDigit % 2 === 1) {
        let decimalAsNumber = parseInt(roundedDecimal, 10) + 1;

        if (decimalAsNumber.toString().length > roundedDecimal.length) {
          const integerAsNumber = parseInt(integerPart, 10) + 1;
          return parseFloat(`${integerAsNumber}.${decimalAsNumber.toString().substring(1)}`);
        }

        roundedDecimal = decimalAsNumber.toString().padStart(digits, '0');
      }
    }

    return parseFloat(`${integerPart}.${roundedDecimal}`);
  }
}
