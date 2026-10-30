namespace jcajamarcaExamen.Models;

public class Socio
{
    public const double CostoMembresia = 1200;
    public const double MontoMinimo = CostoMembresia * 0.10;   // 120
    public const double MontoMaximo = CostoMembresia * 0.50;   // 600
    public const int NumeroCuotas = 5;
    public const double RecargoCuota = CostoMembresia * 0.02;  // 24
    public const double RecargoCasillero = 10;

    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public int Edad { get; set; }
    public DateTime FechaInicio { get; set; }
    public string Plan { get; set; } = "";
    public bool Casillero { get; set; }
    public double MontoInicial { get; set; }

    // REGLA DE NEGOCIO: (1200 - monto inicial) / 5 + 2% del costo + 10 si tiene casillero
    public double PagoMensual =>
        Math.Round((CostoMembresia - MontoInicial) / NumeroCuotas
                   + RecargoCuota
                   + (Casillero ? RecargoCasillero : 0), 2);

    public double PagoTotal => Math.Round(MontoInicial + PagoMensual * NumeroCuotas, 2);
}
