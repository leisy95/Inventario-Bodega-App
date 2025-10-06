import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Cliente, ClienteService } from '../../services/cliente.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-crear-cliente',
  templateUrl: './crear-cliente.component.html',
  styleUrls: ['./crear-cliente.component.css']
})
export class CrearClienteComponent {

  loading = false;

  cliente: Cliente = {
    id: 0,
    nombre: '',
    direccion: '',
    telefono: '',
    email: ''
  };

  constructor(
    private clienteService: ClienteService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<CrearClienteComponent>
  ) { }

  crearCliente() {
    if (!this.cliente.nombre) {
      this.toastr.error('El nombre es obligatorio', 'Error');
      return;
    }

    this.clienteService.crearCliente(this.cliente).subscribe({
      next: (res: any) => {
        this.toastr.success(res.message, 'Éxito');
        this.dialogRef.close(true); // cierra modal
      },
      error: (err) => {
        console.error('Error al registrar cliente:', err);
        this.toastr.error('No se pudo registrar el cliente', 'Error');
      }
    });
  }

  resetForm() {
    this.cliente = { id: 0, nombre: '', direccion: '', telefono: '', email: '' };
  }

  cancelar() {
    this.dialogRef.close(false); 
  }
}