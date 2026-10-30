namespace jcajamarcaExamen.Views;

public partial class vLogin : ContentPage
{
    // Matriz 4 x 2: [usuario, contraseña]
    private readonly string[,] usuarios =
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

	public vLogin()
	{
		InitializeComponent();
	}

    private async void btnIniciarSesion_Clicked(object? sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? "";
        string contrasena = txtContrasena.Text ?? "";

        if (usuario == "" || contrasena == "")
        {
            await DisplayAlertAsync("Error", "Ingrese el usuario y la contraseña", "Cerrar");
            return;
        }

        bool valido = false;
        for (int i = 0; i < usuarios.GetLength(0); i++)
        {
            if (usuarios[i, 0] == usuario && usuarios[i, 1] == contrasena)
            {
                valido = true;
                break;
            }
        }

        if (!valido)
        {
            await DisplayAlertAsync("Error", "Usuario o contraseña incorrectos", "Cerrar");
            return;
        }

        txtContrasena.Text = "";
        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private async void btnAcercaDe_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAcercaDe());
    }
}
