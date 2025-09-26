import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { SalidaService } from '../../services/salida.service';
import { InventarioItemService } from '../../services/inventario-item.service';

@Component({
  selector: 'app-buscar-salidas',
  templateUrl: './buscar-salidas.component.html',
  styleUrls: ['./buscar-salidas.component.css']
})
export class BuscarSalidasComponent implements OnInit {
  salidaActual: any = null;
  filtroReferencia: string = '';
  dataSource = new MatTableDataSource<any>([]);
  displayedColumns: string[] = ['referenciaPeso', 'pesoActual', 'acciones'];

  @ViewChild('inputRef') inputRef!: ElementRef<HTMLInputElement>;

  constructor(
    private salidaService: SalidaService,
    private inventarioService: InventarioItemService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.cargarSalidaActual();
  }

  private cargarSalidaActual() {
    this.salidaService.obtenerSalidaActual().subscribe({
      next: (salida) => {
        this.salidaActual = salida;
        this.dataSource.data = salida.items || [];
      },
      error: () => {
        // Si no hay salida activa, crear una nueva
        this.salidaService.crearSalida().subscribe({
          next: (salida) => {
            this.salidaActual = salida;
            this.dataSource.data = [];
          }
        });
      }
    });
  }

  // Cada vez que el lector escanea algo
  onScanChange(valor: string) {
    if (!valor || valor.trim().length < 3) return;

    this.inventarioService.buscarFIFO(valor).subscribe({
      next: (item) => {
        if (!item) {
          this.toastr.warning('Referencia no encontrada');
          this.resetInput();
          return;
        }

        // Agregar al backend
        console.log('Item a agregar:', item);
        this.salidaService.agregarItem(this.salidaActual.id, item.id).subscribe({
          next: () => {
            this.toastr.success('Item agregado', item.referenciaPeso);
            this.cargarSalidaActual(); // refrescar lista desde backend
            this.resetInput();
          },
          error: (err) => {
            this.toastr.error(err.error?.message || 'Error al agregar item');
            this.resetInput();
          }
        });
      },
      error: (err) => {
        this.toastr.warning(err.error?.message || 'No quedan disponibles');
        this.resetInput();
      }
    });
  }

  quitarSeleccionado(item: any) {
    this.salidaService.quitarItem(this.salidaActual.id, item.idInventarioItem).subscribe({
      next: () => {
        this.toastr.info('Item eliminado', item.referenciaPeso);
        this.cargarSalidaActual();
      }
    });
  }

  confirmarSalida() {
    this.salidaService.confirmarSalida(this.salidaActual.id).subscribe({
      next: () => {
        this.toastr.success('Salida confirmada');
        this.salidaActual = null;
        this.dataSource.data = [];
        this.cargarSalidaActual();
      },
      error: (err) => {
        this.toastr.error(err.error?.message || 'Error al confirmar salida');
      }
    });
  }

  cancelarSalida() {
    this.salidaService.cancelarSalida(this.salidaActual.id).subscribe({
      next: () => {
        this.toastr.warning('Salida cancelada');
        this.salidaActual = null;
        this.dataSource.data = [];
        this.cargarSalidaActual();
      }
    });
  }

  private resetInput() {
    this.filtroReferencia = '';
    setTimeout(() => this.inputRef.nativeElement.focus(), 0);
  }
}