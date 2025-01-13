using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculadoraWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public MainWindow()
		{
			InitializeComponent();

			try
			{
				_mediaPlayer.Open(new Uri(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\songDora.mp3", UriKind.Absolute));
				_mediaPlayer.Volume = 0.1; // Ajustar volumen 


				// Configurar reproducción en bucle
				_mediaPlayer.MediaEnded += (sender, e) =>
				{
					_mediaPlayer.Position = TimeSpan.Zero; // Reiniciar desde el inicio
					_mediaPlayer.Play();
				};

				_mediaPlayer.Play();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al reproducir el audio: {ex.Message}");
			}
		}

		private MediaPlayer _mediaPlayer = new MediaPlayer();

		private void ReproducirSonido(string rutaSonido)
		{
			try
			{
				MediaPlayer soundPlayer = new MediaPlayer();
				soundPlayer.Open(new Uri(rutaSonido, UriKind.Absolute));
				soundPlayer.Volume = 0.5; // Ajusta el volumen si lo deseas
				soundPlayer.Play();

				// Liberar recursos después de reproducir el sonido
				soundPlayer.MediaEnded += (sender, e) =>
				{
					soundPlayer.Close();
				};
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al reproducir el sonido: {ex.Message}");
			}
		}


		private void CambiarFondo(string rutaImagen)
		{
			this.Background = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/{rutaImagen}", UriKind.Absolute)));
		}



		private void btnNumeroClick(object sender, RoutedEventArgs e)
		{
			ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\buttonSound.mp3");
			if (Pantalla.Text.Length < 15) // Si no se supera el límite
			{
				Button boton = sender as Button;
				Pantalla.Text += boton.Content.ToString();
			}
			else
			{
				// Mostrar mensaje de error en el TextBox y cambio de fondo.
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Número demasiado grande.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);

				// Opcional: Restablecer el color y el contenido tras unos segundos
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Borrar el mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});
			}
		}

		private void btnAC_Click(object sender, RoutedEventArgs e)
		{
			ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\buttonSound.mp3");

			// Limpiar el contenido del TextBox
			Pantalla.Text = "";

			// Restablecer el color al original (por si estaba en rojo)
			Pantalla.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void btnOperacionClick(object sender, RoutedEventArgs e)
		{
			ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\buttonSound.mp3");

			string contenido = Pantalla.Text;

			// Verificar si el `TextBox` está vacío
			if (string.IsNullOrEmpty(contenido))
			{

				// Mostrar mensaje de error en rojo
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Ingresa un número primero.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);

				// Restaurar el TextBox después de 2 segundos
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Limpiar mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});

				return;
			}

			// Verificar si ya hay un operador
			if (contenido.Contains("+") || contenido.Contains("-") || contenido.Contains("x") || contenido.Contains("/"))
			{
				// Mostrar mensaje de error en rojo
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Ya hay una operación.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);

				// Restaurar el TextBox después de 2 segundos
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Limpiar mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});

				return;
			}

			// Agregar el operador seleccionado al `TextBox`
			Button boton = sender as Button;
			Pantalla.Text += boton.Content.ToString();
		}

		private void btnIgualClick(object sender, RoutedEventArgs e)
		{
			ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\buttonSound.mp3");

			string contenido = Pantalla.Text; // Contenido del TextBox
			double d1, d2, resultado;
			char operacion = '\0'; // Inicializamos con un valor vacío

			// Verificar que el contenido no esté vacío
			if (string.IsNullOrEmpty(contenido))
			{
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Ingresa una operación.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Limpiar mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});
				return;
			}

			// Identificar operador (+, -, x, /) y separar los números
			foreach (char c in contenido)
			{
				if (c == '+' || c == '-' || c == 'x' || c == '/')
				{
					operacion = c;
					break;
				}
			}

			// Si no se encontró un operador, mostrar error
			if (operacion == '\0')
			{
				
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Operación inválida.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Limpiar mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});
				return;
			}

			// Separar los números antes y después del operador
			string[] partes = contenido.Split(operacion);
			if (partes.Length != 2 || !double.TryParse(partes[0], out d1) || !double.TryParse(partes[1], out d2))
			{
				CambiarFondo("Assets/FondoError.png");
				ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
				Pantalla.Text = "Error en los números.";
				Pantalla.Foreground = new SolidColorBrush(Colors.Red);
				Task.Delay(2000).ContinueWith(_ =>
				{
					Dispatcher.Invoke(() =>
					{
						CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
						Pantalla.Text = ""; // Limpiar mensaje
						Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
					});
				});
				return;
			}

			// Realizar el cálculo según el operador
			switch (operacion)
			{
				case '+':
					resultado = d1 + d2;
					break;
				case '-':
					resultado = d1 - d2;
					break;
				case 'x':
					resultado = d1 * d2;
					break;
				case '/':
					if (d2 == 0)
					{
						CambiarFondo("Assets/FondoError.png");
						ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
						Pantalla.Text = "No se puede dividir entre 0.";
						Pantalla.Foreground = new SolidColorBrush(Colors.Red);
						Task.Delay(2000).ContinueWith(_ =>
						{
							Dispatcher.Invoke(() =>
							{
								CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
								Pantalla.Text = ""; // Limpiar mensaje
								Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
							});
						});
						return;
					}
					resultado = d1 / d2;
					break;
				default:
					CambiarFondo("Assets/FondoError.png");
					ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\errorSound.mp3");
					Pantalla.Text = "Operador inválido.";
					Pantalla.Foreground = new SolidColorBrush(Colors.Red);
					Task.Delay(2000).ContinueWith(_ =>
					{
						Dispatcher.Invoke(() =>
						{
							CambiarFondo("Assets/FondoNeutral.png"); // Volver al fondo neutral
							Pantalla.Text = ""; // Limpiar mensaje
							Pantalla.Foreground = new SolidColorBrush(Colors.Black); // Restaurar color
						});
					});
					return;
			}

			// Cambiar al fondo de éxito
			CambiarFondo("Assets/FondoExito.png");
			ReproducirSonido(@"C:\Users\KEKOO\Desktop\Curso Lógica\CalculadoraWpf\Assets\ExitoSound.mp3");


			// Programar el retorno al fondo neutral tras 3 segundos
			Task.Delay(3000).ContinueWith(_ =>
			{
				Dispatcher.Invoke(() =>
				{
					CambiarFondo("Assets/FondoNeutral.png");
				});
			});

			// Mostrar el resultado en el TextBox
			Pantalla.Text = resultado.ToString();
			Pantalla.Foreground = new SolidColorBrush(Colors.Black);

		}




	}
}