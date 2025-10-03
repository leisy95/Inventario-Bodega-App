import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Cliente, ClienteService } from '../../services/cliente.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit {
  cliente: Cliente = {   
    nombre: '',
    direccion: '',
    telefono: '',
    email: ''
  };

  clientes: Cliente[] = [];
  displayedColumns: string[] = ['id', 'nombre', 'direccion', 'telefono', 'email'];
  loading = false;

  constructor(
    private clienteService: ClienteService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.obtenerClientes();
  }

  crearCliente() {
    if (!this.cliente.nombre) {
      this.toastr.error('El nombre es obligatorio', 'Error');
      return;
    }

    this.clienteService.crearCliente(this.cliente).subscribe({
      next: (res: any) => {
        this.toastr.success(res.message, 'Éxito');
        this.resetForm();
        this.obtenerClientes();
      },
      error: (err) => {
        console.error('Error al registrar cliente:', err);
        this.toastr.error('No se pudo registrar el cliente', 'Error');
      }
    });
  }

  obtenerClientes() {
    this.clienteService.listarClientes().subscribe({
      next: (data) => (this.clientes = data),
      error: (err) => console.error('Error al obtener clientes:', err)
    });
  }

  resetForm() {
    this.cliente = { nombre: '', direccion: '', telefono: '', email: '' };
  }
}