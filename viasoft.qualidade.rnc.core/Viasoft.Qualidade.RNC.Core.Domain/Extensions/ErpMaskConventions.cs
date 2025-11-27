using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.Extensions;

public static class ErpMaskConventions
{
    public static string AddDateMask(this DateTime value)
    {
        var year = value.Year.ToString();
        var month = value.Month.ToString("00");
        var day = value.Day.ToString("00");
        return year + month + day;
    }
}