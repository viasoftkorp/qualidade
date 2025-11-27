export class StringUtils {
  public static isNumber(string: string):boolean {
    const isNumber = !Number.isNaN(Number.parseInt(string, 10));
    return isNumber;
  }
}
