import { VsMaskBase } from '@viasoft/components';

export function setMaskDecimalPlace(casasDecimais: number, padFractionalZeros = true, thousandsSeparator = '.'): unknown {
  return {
    mask: Number,
    scale: casasDecimais,
    signed: false,
    padFractionalZeros,
    normalizeZeros: true,
    min: 0,
    max: 999999999,
    radix: ',',
    thousandsSeparator
  };
}

export function getMascaraCampoNumerico(
  casasDecimais: number,
  padFractionalZeros = true,
  thousandsSeparator = '.'
): VsMaskBase {
  return {
    type: 'custom',
    imask: setMaskDecimalPlace(casasDecimais, padFractionalZeros, thousandsSeparator)
  } as VsMaskBase;
}
