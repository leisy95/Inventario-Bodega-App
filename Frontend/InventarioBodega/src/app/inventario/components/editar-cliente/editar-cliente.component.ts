import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ClienteService, EditarClienteRequest, Cliente } from '../../services/cliente.service';

@Component({
  selector: 'app-editar-cliente',
  templateUrl: './editar-cliente.component.html',
  styleUrls: ['./editar-cliente.component.css']
})
export class EditarClienteComponent implements OnInit {
  clienteForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<EditarClienteComponent>,
    @Inject(MAT_DIALOG_DATA) public cliente: Cliente
  ) {
    // Inicializa el formulario con los datos del cliente recibido
    this.clienteForm = this.fb.group({
      nombre: [this.cliente.nombre, [Validators.required, Validators.maxLength(150)]],
      direccion: [this.cliente.direccion, ],
      telefono: [this.cliente.telefono,],
      email: [this.cliente.email, [Validators.maxLength(100), Validators.email]]
    });
  }

  ngOnInit(): void { }

  guardarCambios() {
    if (this.clienteForm.invalid) {
      this.toastr.error('Por favor completa los campos correctamente', 'Error');
      return;
    }

    const clienteActualizado = {
      id: this.cliente.id!,
      nombre: this.clienteForm.value.nombre,
      direccion: this.clienteForm.value.direccion,
      telefono: this.clienteForm.value.telefono,
      email: this.clienteForm.value.email
    };

    this.clienteService.actualizarCliente(this.cliente.id!, clienteActualizado)
      .subscribe({
        next: () => {
          this.toastr.success('Cliente actualizado con éxito', 'Éxito');
          this.dialogRef.close(true); // Cierra el diálogo y notifica al componente padre
        },
        error: (err) => {
          console.error('Error al actualizar cliente:', err);
          this.toastr.error('No se pudo actualizar el cliente', 'Error');
        }
      });
  }

  cancelar() {
    this.dialogRef.close(false); // Cierra el diálogo sin hacer cambios
  }
}