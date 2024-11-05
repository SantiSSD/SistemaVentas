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
    public partial class frmCategoria : Form
    {
        public frmCategoria()
        {
            InitializeComponent();
        }

        private void frmCategoria_Load(object sender, EventArgs e)
        {
            {
                dgvData.Rows.Clear();
                // Inicializar una lista de IDs para verificar duplicados
                HashSet<int> usuariosAgregados = new HashSet<int>(); // Aquí se crea el HashSet

                cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
                cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
                cboEstado.DisplayMember = "Texto";
                cboEstado.ValueMember = "Valor";
                cboEstado.SelectedIndex = 0;

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

                List<Categoria> lista = new CN_Categoria().Listar();

                foreach (Categoria usuario in lista)
                {
                    if (!usuariosAgregados.Contains(usuario.IdCategoria))
                    {
                        // Si no está, agregar al DataGridView
                        dgvData.Rows.Add(new object[]
                        {
                        "",
                        usuario.IdCategoria,
                        usuario.Descripcion,
                        usuario.Estado == true ? 1 : 0,
                        usuario.Estado == true ? "Activo" : "Inactivo",
                        });

                        // Añadir al HashSet
                        usuariosAgregados.Add(usuario.IdCategoria);
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            {
                string mensaje = string.Empty;

                int idCategoria = 0; // Declarar variable para el ID

                // Intentar convertir el valor de txtId.Text a un entero
                if (!int.TryParse(txtId.Text, out idCategoria))
                {
                    idCategoria = 0; // Si no se puede convertir, asignar 0 por defecto
                }

                Categoria obj = new Categoria()
                {
                    IdCategoria = idCategoria,
                    Descripcion = txtDescripcion.Text,
                    Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1
                };

                if (obj.IdCategoria == 0)
                {
                    // Llamar al método Registrar si es un nuevo registro
                    int idgenerado = new CN_Categoria().Registrar(obj, out mensaje);

                    if (idgenerado != 0)
                    {
                        dgvData.Rows.Add(new object[]
                        {
                "",
                idgenerado,
                txtDescripcion.Text,
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
                    // Llamar al método Editar si es un registro existente
                    bool resultado = new CN_Categoria().Editar(obj, out mensaje);

                    if (resultado)
                    {
                        DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                        row.Cells["Id"].Value = txtId.Text;
                        row.Cells["Descripcion"].Value = txtDescripcion.Text;
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

        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = ""; // Vaciar el campo
            txtDescripcion.Text = ""; // Vaciar el campo
            cboEstado.SelectedIndex = 0; // Seleccionar el primer ítem de la lista

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
                    txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();

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
                if (MessageBox.Show("¿Deseas eliminar la categoria?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Categoria obj = new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(txtId.Text)
                    };
                    bool respuesta = new CN_Categoria().Eliminar(obj, out mensaje);

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
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}






