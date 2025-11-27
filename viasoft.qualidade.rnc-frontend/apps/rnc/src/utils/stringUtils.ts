export class StringUtils {
  public static isNumber(string: string):boolean {
    const isNumberRegex = /^\d+$/;
    const isNumber = isNumberRegex.test(string);
    return isNumber;
  }
}
