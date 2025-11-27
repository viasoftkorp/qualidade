import { FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';

export function tempoTotalValidator(minutosFormControlName: string, horasFormControlName: string): ValidatorFn {
  return (formGroup: FormGroup): ValidationErrors | null => {
    const minutosFormControl = formGroup.get(minutosFormControlName);

    const horasFormControl = formGroup.get(horasFormControlName);

    if (minutosFormControl && horasFormControl) {
      const minutosValor = minutosFormControl.value as number | string;
      const horasValor = horasFormControl.value as number | string;

      // Não verificado tipo da variavel para evitar erros em caso de "0" como número ou como string
      // eslint-disable-next-line eqeqeq
      if (minutosValor != 0 || horasValor != 0) {
        return null;
      }

      return { tempoTotalInvalido: true };
    }

    return null;
  };
}
