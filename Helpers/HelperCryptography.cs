using System.Security.Cryptography;
using System.Text;

namespace MvcCoreUtilidades.Helpers
{
    public class HelperCryptography
    {
        //Tendremos una propiedad donde almacenaremos el salt dinamico que vamos a generar
        public static string Salt { get; set; }

        //Cada vez que realicemos un cifrado, se genera un salt distinto
        private static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                //Un numero entre los caracteres ASCII
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        //Creamos un metodo para cifrar correctamente
        public static string EncriptarContenido(string contenido, bool comparar)
        {
            //password123
            if (comparar == false)
            {
                //El usuario quiere cifarar, por lo que generamos un nuevo salt y lo guardamos en la propiedad
                Salt = GenerateSalt();
            }
            //El Salt se utuliza en multiples lugares, es decir, lo podemos incluir al final, al inicio, con un insert...
            string contenidoSalt = contenido + Salt;

            //Creamos el objeto para cifrar el contenido
            SHA256 sHA256 = SHA256.Create();
            byte[] salida;
            UnicodeEncoding encoding = new UnicodeEncoding();

            //Convertimos a bytes nuestro contenido + salt
            salida = encoding.GetBytes(contenidoSalt);

            //Ciframos el contenido N interaciones
            for (int i = 0; i <= 25; i++)
            {
                //Realizamos cifrado sobre cifrado
                salida = sHA256.ComputeHash(salida);
            }

            //Debemos limpiar el objeto del cifrado
            sHA256.Clear();

            string resultado = encoding.GetString(salida);
            return resultado;
        }

        //Vamos a crear un metodo static para convertir un contenido y cifrarlo
        //Devolvemos el texto cifrado
        public static string EncriptarTextoBasico(string contenido)
        {
            //Necesitamos un array de bytes para convertir el texto a byte[]
            byte[] entrada;

            //al cifrar, nos devolvera un array de bytes con la salida cifrada
            byte[] salida;

            //Necesitamos una clase que nos permite convertir string a byte[] y viceversa
            UnicodeEncoding encoding = new UnicodeEncoding();

            //Necesitamos el objeto para cifrar el contenido
            SHA1Managed managed = new SHA1Managed();

            //SHA1 sha = SHA1.Create()
            //Convertimos el string del contenido a byte[]
            entrada = encoding.GetBytes(contenido);

            //el objeto SHA1 recibe un array de bytes e internamente modifica cada elemento devolviendo otro array de bytes
            salida = managed.ComputeHash(entrada);

            //Convertimos bytes[] a string
            string resultado = encoding.GetString(salida);
            return resultado;
        }
    }
}
