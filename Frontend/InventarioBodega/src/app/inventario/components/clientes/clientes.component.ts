import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Cliente, ClienteService } from '../../services/cliente.service';
import { MatDialog } from '@angular/material/dialog';
import { EditarClienteComponent } from '../editar-cliente/editar-cliente.component';
import { CrearClienteComponent } from '../crear-cliente/crear-cliente.component';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit {
  clientes: Cliente[] = [];
  cliente: Cliente = { id: 0, nombre: '', direccion: '', telefono: '', email: '' };
  displayedColumns: string[] = ['nombre', 'direccion', 'telefono', 'email', 'acciones'];
  loading = false;

  constructor(
    private clienteService: ClienteService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.obtenerClientes();
  }

  // Listar clientes
  obtenerClientes() {
    this.clienteService.listarClientes().subscribe({
      next: (data) => this.clientes = data,
      error: (err) => console.error('Error al obtener clientes:', err)
    });
  }

  ModalCrearCliente() {
    const dialogRef = this.dialog.open(CrearClienteComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe((result: boolean | undefined) => {
      if (result === true) {
        this.toastr.success('cliente creado', 'Éxito');
        this.obtenerClientes();
      }
    });
  }

  // Editar cliente: abre modal
  editarCliente(cliente: Cliente) {
    const dialogRef = this.dialog.open(EditarClienteComponent, {
      width: '400px',           // ancho
      panelClass: 'custom-dialog',
      data: cliente
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Si result es true, recarga la lista
        this.obtenerClientes();
      }
    });
  }

  // Eliminar cliente
  eliminarCliente(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        title: 'Eliminar Cliente',
        message: '¿Seguro que deseas eliminar este cliente?'
      } as ConfirmDialogData
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Si el usuario confirma, eliminar cliente
        this.clienteService.eliminarCliente(id).subscribe({
          next: (res) => {
            this.toastr.success(res.message, 'Éxito');
            this.obtenerClientes();
          },
          error: (err) => {
            console.error('Error al eliminar cliente:', err);
            this.toastr.error('No se pudo eliminar el cliente', 'Error');
          }
        });
      }
    });
  }
}