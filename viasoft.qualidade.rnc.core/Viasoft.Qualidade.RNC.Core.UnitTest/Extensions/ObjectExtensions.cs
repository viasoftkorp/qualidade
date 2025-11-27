using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.UnitTest.Extensions;

public static class ObjectExtensions
{
    public static bool IsEquivalentTo(this object objectA, object objectB)
    {
        return IsEquivalent(objectA, objectB, new List<string>());
    }
    public static bool IsEquivalentTo(this object objectA, object objectB, List<string> excludingProperties )
    {
        return IsEquivalent(objectA, objectB, excludingProperties);
    }

    private static bool IsEquivalent(object objetoA, object objetoEsperado, List<string> excludingProperties)
    {
        Type tipoObjetoA = objetoA.GetType();
        Type tipoObjetoEsperado = objetoEsperado.GetType();

        if (tipoObjetoA != tipoObjetoEsperado)
        {
            Console.WriteLine($"Entidades não são equivalentes" +
                                        $"Entidade A: {tipoObjetoA}" +
                                        $"Entidade B: {tipoObjetoEsperado} ");
            return false;
        }


        if (IsList(tipoObjetoA))
        {
            return ListEquivalentTo(objetoA, objetoEsperado, excludingProperties);
        }

        PropertyInfo[] propriedades = tipoObjetoA.GetProperties();
    
        foreach (PropertyInfo propriedade in propriedades)
        {
            if (excludingProperties.Contains(propriedade.Name))
            {
                continue;
            }
            
            var isTipoPrimitivo = propriedade.PropertyType.IsPrimitive || propriedade.PropertyType.IsValueType || propriedade.PropertyType == typeof(string);
            
            if (!isTipoPrimitivo)
            {
                object objetoAninhado = propriedade.GetValue(objetoA);
                object expectedObjetoAninhado = propriedade.GetValue(objetoEsperado);
                var isEquivalent = IsEquivalent(objetoAninhado, expectedObjetoAninhado, excludingProperties);

                if (!isEquivalent)
                {
                    return false;
                }

                continue;
            }
            
            object valorPropriedade = propriedade.GetValue(objetoA);
            object valorPropriedadeEsperado = propriedade.GetValue(objetoEsperado);

            var tipoPropriedade = propriedade.PropertyType;
            if (IsList(tipoPropriedade))
            {
                return ListEquivalentTo(valorPropriedade, valorPropriedadeEsperado, excludingProperties);
            }

            var propriedadesIguais = Equals(valorPropriedade, valorPropriedadeEsperado);
            
            if (!propriedadesIguais)
            {
                Console.Write($"Os valores da propriedade {propriedade.Name} divergem" +
                              $"Entidade A: {valorPropriedade}" +
                              $"Entidade B: {valorPropriedadeEsperado}");
                return false;
            }
        }

        return true;
    }

    private static bool ListEquivalentTo(object objectA, object objectB, List<string> excludingProperties)
    {
        IList listA = (IList)objectA;
        IList listB = (IList)objectB;

        if (listA.Count != listB.Count)
        {
            Console.Write($"As listas não têm o mesmo número de elementos" +
                          $"Lista A: {listA.Count}" +
                          $"Lista B: {listB.Count}");
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (!IsEquivalent(listA[i], listB[i], excludingProperties))
            {
                Console.Write($"Elementos das listas não são equivalentes na posição {i}");
                return false;
            }
        }

        return true;
    }

    private static bool IsList(Type tipo)
    {
        return tipo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(List<>);
    }
}