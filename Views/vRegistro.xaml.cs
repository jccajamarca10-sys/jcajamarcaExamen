using System.Globalization;
using System.Text.RegularExpressions;
using jcajamarcaExamen.Models;

namespace jcajamarcaExamen.Views;

public partial class vRegistro : ContentPage
{
    private readonly string usuario;
    private Socio? socioCalculado;

	public vRegistro(string usuario)
	{
		InitializeComponent();
        this.usuario = usuario;
        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;

        // No permitir fechas pasadas
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
	}

    private async void btnCalcular_Clicked(object? sender, EventArgs e)
    {
        string? error = Validar(out Socio socio);
        if (error != null)
        {
            await DisplayAlertAsync("Error", error, "Cerrar");
            return;
        }

        socioCalculado = socio;
        txtPagoMensual.Text = socio.PagoMensual.ToString("F2");
    }

    private async void btnVerResumen_Clicked(object? sender, EventArgs e)
    {
        if (socioCalculado == null)
        {
            await DisplayAlertAsync("Error", "Primero presione Calcular Pago mensual", "Cerrar");
            return;
        }

        // Volver a validar por si cambiaron nombre, edad, fecha o plan despues de calcular
        string? error = Validar(out Socio socio);
        if (error != null)
        {
            await DisplayAlertAsync("Error", error, "Cerrar");
            return;
        }

        await Navigation.PushAsync(new vResumen(usuario, socio));
    }

    // Si cambia el monto o el casillero, el pago calculado ya no es valido
    private void txtMontoInicial_TextChanged(object? sender, TextChangedEventArgs e) => BorrarCalculo();

    private void swCasillero_Toggled(object? sender, ToggledEventArgs e) => BorrarCalculo();

    private void BorrarCalculo()
    {
        socioCalculado = null;
        txtPagoMensual.Text = "";
    }

    // Devuelve el mensaje de error, o null si todo es valido
    private string? Validar(out Socio socio)
    {
        socio = new Socio();
        string letras = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]+$";

        string nombre = txtNombre.Text?.Trim() ?? "";
        if (!Regex.IsMatch(nombre, letras))
            return "Ingrese el nombre (solo letras)";

        string apellido = txtApellido.Text?.Trim() ?? "";
        if (!Regex.IsMatch(apellido, letras))
            return "Ingrese el apellido (solo letras)";

        string textoEdad = txtEdad.Text?.Trim() ?? "";
        if (!Regex.IsMatch(textoEdad, @"^\d{1,3}$"))
            return "La edad debe contener solo números";
        int edad = int.Parse(textoEdad);
        if (edad < 16)
            return "La edad debe ser mayor o igual a 16 años";
        if (edad > 100)
            return "Ingrese una edad válida (máximo 100)";

        if (dpFechaInicio.Date == null || dpFechaInicio.Date.Value.Date < DateTime.Today)
            return "La fecha de inicio no puede ser una fecha pasada";

        if (pkPlan.SelectedIndex == -1)
            return "Seleccione un plan";

        string textoMonto = (txtMontoInicial.Text ?? "").Trim().Replace(',', '.');
        if (!double.TryParse(textoMonto, NumberStyles.Float, CultureInfo.InvariantCulture, out double monto))
            return "El monto inicial debe ser numérico";
        if (monto < Socio.MontoMinimo || monto > Socio.MontoMaximo)
            return "El monto inicial debe estar entre 120 y 600";

        socio.Nombre = nombre;
        socio.Apellido = apellido;
        socio.Edad = edad;
        socio.FechaInicio = dpFechaInicio.Date.Value;
        socio.Plan = pkPlan.SelectedItem?.ToString() ?? "";
        socio.Casillero = swCasillero.IsToggled;
        socio.MontoInicial = monto;
        return null;
    }
}
