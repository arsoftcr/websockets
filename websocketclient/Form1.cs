using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace websocketclient
{
    public partial class Form1 : Form
    {
        bool enviado = false;

        ClientWebSocket cliente = new ClientWebSocket();


        public Form1()
        {
            InitializeComponent();
        }

        private  void Button1_Click(object sender, EventArgs e)
        {
            enviado = true;

         
            var enviar = Task.Run(async () => {

    
                    var bytes = Encoding.UTF8.GetBytes(textBox1.Text);



                    await cliente.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text,
                        true, CancellationToken.None);

                    enviado = false;
                

               // await cliente.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Socket cerrado", CancellationToken.None);

            });


            Task.WhenAll(enviar);
        }


        private  async Task conectarServidor(string men)
        {
           

          

            await cliente.ConnectAsync(new Uri("ws://localhost:5000/ws"),CancellationToken.None);



            var recibido = ReceiveAsync(cliente);


            await Task.WhenAll(recibido);
        }


        private  async Task ReceiveAsync(ClientWebSocket cliente)
        {
            await Task.Run(()=> {

                Invoke(new MethodInvoker(async () =>
                {

                    var buffer = new byte[1024 * 4];



                    while (true)
                    {
                        var resultado = await cliente.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);





                        richTextBox1.Text += "\n" + Encoding.UTF8.GetString(buffer, 0, resultado.Count);




                        if (resultado.MessageType == WebSocketMessageType.Close)
                        {
                            await cliente.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

                            break;
                        }


                    }

                }));




            });
           
          

            
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await conectarServidor(textBox1.Text);
        }
    }
}
