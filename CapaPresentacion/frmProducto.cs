using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmProducto : Form
    {
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();
            // Inicializar una lista de IDs para verificar duplicados
            HashSet<int> productosAgregados = new HashSet<int>(); // Aquí se crea el HashSet

            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            List<Categoria> ListaCategoria = new CN_Categoria().Listar();
            cboCategoria.Items.Clear();
            foreach (Categoria item in ListaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.IdCategoria, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;

            cboBusqueda.Items.Clear();
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            //MOSTRAR TODOS LOS USUARIOS
            List<Producto> listaProducto = new CN_Producto().Listar();

            foreach (Producto producto in listaProducto)
            {
                if (!productosAgregados.Contains(producto.IdProducto))
                {
                    // Si no está, agregar al DataGridView
                    dgvData.Rows.Add(new object[]
                    {
                        "",
                        producto.IdProducto,
                        producto.Codigo,
                        producto.Nombre,
                        producto.Descripcion,
                        producto.oCategoria.IdCategoria,
                        producto.oCategoria.Descripcion,
                        producto.Stock,
                        producto.PrecioCompra,
                        producto.PrecioVenta,
                        producto.Estado == true ? 1 : 0,
                        producto.Estado == true ? "Activo" : "Inactivo",
                    });

                    // Añadir al HashSet
                    productosAgregados.Add(producto.IdProducto);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;


            Producto objproducto = new Producto()
            {
                IdProducto = Convert.ToInt32(txtId.Text),
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false

            };



            if (objproducto.IdProducto == 0)
            {

                int idproductogenerado = new CN_Producto().Registrar(objproducto, out mensaje);

                if (idproductogenerado != 0)
                {
                    dgvData.Rows.Add(new object[]
                    {
                        "",
                        idproductogenerado,
                        txtCodigo.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,
                        ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString(),
                        "0",
                        "0.00",
                        "0.00",


                        ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString(),
                    });

                    Limpiar();

                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
            else
            {
                bool resultado = new CN_Producto().Editar(objproducto, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString();


                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();


                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = ""; // Vaciar el campo
            txtCodigo.Text = ""; // Vaciar el campo
            txtNombre.Text = ""; // Vaciar el campo
            txtDescripcion.Text = ""; // Vaciar el campo
            cboCategoria.SelectedIndex = 0; // Seleccionar el primer ítem de la lista
            cboEstado.SelectedIndex = 0; // Seleccionar el primer ítem de la lista
            txtCodigo.Select();

        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)

                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }

        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtCodigo.Text = dgvData.Rows[indice].Cells["Codigo"].Value.ToString();
                    txtNombre.Text = dgvData.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();

                    foreach (OpcionCombo oc in cboCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdCategoria"].Value))
                        {
                            int indice_combo = cboCategoria.Items.IndexOf(oc);
                            cboCategoria.SelectedIndex = indice_combo;
                            break;
                        }

                    }

                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indice_combo;
                            break;
                        }

                    }

                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Deseas eliminar este producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Producto objproducto = new Producto()
                    {
                        IdProducto = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = new CN_Producto().Eliminar(objproducto, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))

                        row.Visible = true;

                    else

                        row.Visible = false;

                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
