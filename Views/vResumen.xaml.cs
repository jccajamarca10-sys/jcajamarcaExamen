using jcajamarcaExamen.Models;

namespace jcajamarcaExamen.Views;

public partial class vResumen : ContentPage
{
    private readonly string usuario;
    private readonly Socio socio;

	public vResumen(string usuario, Socio socio)
	{
		InitializeComponent();
        this.usuario = usuario;
        this.socio = socio;

        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;
        lblNombre.Text = socio.Nombre;
        lblApellido.Text = socio.Apellido;
        lblEdad.Text = socio.Edad + " años";
        lblFechaInicio.Text = socio.FechaInicio.ToString("dd/MM/yyyy");
        lblPlan.Text = socio.Plan;
        lblCasillero.Text = socio.Casillero ? "Sí" : "No";
        lblMontoInicial.Text = "$ " + socio.MontoInicial.ToString("F2");
        lblPagoMensual.Text = "$ " + socio.PagoMensual.ToString("F2");
        lblPagoTotal.Text = "$ " + socio.PagoTotal.ToString("F2");
	}

    private async void btnExportar_Clicked(object? sender, EventArgs e)
    {
        try
        {
            string contenido =
                "RESUMEN DEL SOCIO\n" +
                "-----------------\n" +
                "Usuario conectado: " + usuario + "\n" +
                "Nombre: " + socio.Nombre + "\n" +
                "Apellido: " + socio.Apellido + "\n" +
                "Edad: " + socio.Edad + " años\n" +
                "Fecha de inicio: " + socio.FechaInicio.ToString("dd/MM/yyyy") + "\n" +
                "Plan: " + socio.Plan + "\n" +
                "Casillero: " + (socio.Casillero ? "Sí" : "No") + "\n" +
                "Monto Inicial: $ " + socio.MontoInicial.ToString("F2") + "\n" +
                "Pago mensual: $ " + socio.PagoMensual.ToString("F2") + "\n" +
                "Pago Total: $ " + socio.PagoTotal.ToString("F2") + "\n";

            string nombreArchivo = "socio_" + socio.Nombre.Replace(" ", "") + "_" +
                                   socio.Apellido.Replace(" ", "") + "_" +
                                   DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

#if WINDOWS
            // En Windows se guarda en la carpeta del proyecto
            string carpeta = @"C:\DESARROLLOS\aplicacionesMobiles\jcajamarcaEP";
#else
            // En Android no existe la ruta C:\, se guarda dentro de la app
            string carpeta = FileSystem.AppDataDirectory;
#endif
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta, nombreArchivo);
            await File.WriteAllTextAsync(ruta, contenido);

            await DisplayAlertAsync("Archivo exportado",
                "Nombre del archivo:\n" + nombreArchivo + "\n\n" +
                "Ruta:\n" + carpeta,
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }

    private void btnCerrarSesion_Clicked(object? sender, EventArgs e)
    {
        // Reemplaza toda la navegacion por un Login nuevo
        if (Window != null)
            Window.Page = new NavigationPage(new vLogin());
    }
}
