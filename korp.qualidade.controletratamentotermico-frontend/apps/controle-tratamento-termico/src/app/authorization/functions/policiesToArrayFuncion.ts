export function policiesToArray(policy: object): Array<string> {
  const todasAsPropriedades: Array<string> = [];
  for (const propriedade in policy) {
    const isClasseFilha = typeof policy[propriedade] === "function";

    if (isClasseFilha) {
      const classeFilha = policy[propriedade];
      const propriedadesClasseFilha = policiesToArray(classeFilha)
      todasAsPropriedades.push(...propriedadesClasseFilha)

    } else {
      todasAsPropriedades.push(policy[propriedade]);
    }

  }

  const result = todasAsPropriedades.filter(distinctFunction);
  return result;
}

function distinctFunction(value: string, index: number, self:Array<string>) {
  return self.indexOf(value) === index
}
