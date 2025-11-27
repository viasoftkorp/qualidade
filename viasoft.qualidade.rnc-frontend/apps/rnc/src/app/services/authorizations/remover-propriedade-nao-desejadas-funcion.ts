export function removerPropriedadesNaoDesejadas(properties: Array<string>): Array<string> {
  const propriedadeNaoDesejadas = ['length', 'name', 'prototype', 'toArray', 'removerPropriedadesNaoDesejadas'];
  const result = properties.filter((property) => !propriedadeNaoDesejadas.includes(property));
  return result;
}
