using CapaEntidad;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmCompras : Form
    {
        private Usuario _Usuario;


        public frmCompras(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {
            cbotipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbotipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipoDocumento.DisplayMember = "Texto";
            cbotipoDocumento.ValueMember = "Valor";
            cbotipoDocumento.SelectedIndex = 0;


            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtIdproveedor.Text = "0";
            txtidProduto.Text = "0";

        }

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtIdproveedor.Text = modal._Proveedor.IdProveedor.ToString();
                    txtDocProveedor.Text = modal._Proveedor.Documento;
                    txtnombreProveedor.Text = modal._Proveedor.RazonSocial;
                }
                else
                {
                    txtDocProveedor.Select();
                }

            }
        }

        private void btnbuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();



                if (result == DialogResult.OK)
                {
                    txtidProduto.Text = modal._Producto.IdProducto.ToString();
                    txtcodproducto.Text = modal._Producto.Codigo;
                    txtproducto.Text = modal._Producto.Nombre;
                    txtprecioCompra.Select();
                }
                else
                {
                    txtcodproducto.Select();
                }
            }
        }
    }
}