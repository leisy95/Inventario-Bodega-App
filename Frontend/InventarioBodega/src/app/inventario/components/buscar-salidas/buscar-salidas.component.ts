import { Component, ViewChild, ElementRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { InventarioItemService } from '../../services/inventario-item.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-buscar-salidas',
  templateUrl: './buscar-salidas.component.html',
  styleUrls: ['./buscar-salidas.component.css']
})
export class BuscarSalidasComponent {
  filtroReferencia: string = '';
  itemsSeleccionados: any[] = [];
  dataSource = new MatTableDataSource<any>([]);
  displayedColumns: string[] = ['referenciaPeso', 'pesoActual', 'acciones'];

  @ViewChild('inputRef') inputRef!: ElementRef<HTMLInputElement>;

  constructor(
    private inventarioItemService: InventarioItemService,
    private toastr: ToastrService
  ) { }

  // Cada vez que el lector escanee algo
  onScanChange(valor: string) {
    if (!valor || valor.trim().length < 3) return;

    // Aquí usamos el endpoint FIFO que devuelve SOLO el más antiguo disponible
    this.inventarioItemService.buscarFIFO(valor).subscribe({
      next: (item) => {
        if (item) {
          this.itemsSeleccionados.push(item); // se permite repetir si existen varios en bodega
          this.dataSource.data = [...this.itemsSeleccionados];
          this.toastr.success('Referencia agregada', item.referenciaPeso);
        } else {
          this.toastr.warning('No se encontró la referencia');
        }
        this.resetInput();
      },
      error: () => {
        this.toastr.warning('No quedan más disponibles en almacén');
        this.resetInput();
      }
    });
  }

  enviarSalidas() {
    if (this.itemsSeleccionados.length === 0) {
      this.toastr.warning('No hay referencias seleccionadas');
      return;
    }

    const referencias = this.itemsSeleccionados.map(x => x.referenciaPeso);

    this.inventarioItemService.darSalidas(referencias).subscribe({
      next: () => {
        this.toastr.success('Salidas procesadas correctamente');
        this.itemsSeleccionados = [];
        this.dataSource.data = [];
        this.resetInput();
      },
      error: () => {
        this.toastr.error('Error al procesar las salidas');
      }
    });
  }

  quitarSeleccionado(index: number) {
    const item = this.itemsSeleccionados[index];
    this.itemsSeleccionados.splice(index, 1);
    this.dataSource.data = [...this.itemsSeleccionados];
    this.toastr.info('Referencia eliminada', item.referenciaPeso);
  }

  private resetInput() {
    this.filtroReferencia = '';
    setTimeout(() => this.inputRef.nativeElement.focus(), 0);
  }
}