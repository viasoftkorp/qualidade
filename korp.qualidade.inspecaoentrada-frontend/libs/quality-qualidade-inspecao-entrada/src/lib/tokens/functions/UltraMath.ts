export class UltraMath
{
  public static Sum<T>(arrayToSum: T[], getValue: (item: T) => number): number {
    return arrayToSum.reduce((total, item) => {
      return total + getValue(item);
    }, 0);
  }
}
