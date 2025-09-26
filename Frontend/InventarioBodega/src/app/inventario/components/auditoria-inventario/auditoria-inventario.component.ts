import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { MovimientoInventario, MovimientoInventarioService } from '../../services/movimiento-inventario.service';

interface ResumenInventario {
  titulo: string;
  cantidad: number;
  valor: number;
  colorClass: string;
}

@Component({
  selector: 'app-auditoria-inventario',
  templateUrl: './auditoria-inventario.component.html',
  styleUrls: ['./auditoria-inventario.component.css']
})
export class AuditoriaInventarioComponent implements OnInit {

  resumen: ResumenInventario[] = [];
  dataSource = new MatTableDataSource<MovimientoInventario>();
  displayedColumns: string[] = ['referencia', 'tipo', 'peso', 'fecha', 'usuario', 'inventario', 'item'];

  fechaInicio?: Date;
  fechaFin?: Date;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private movimientoService: MovimientoInventarioService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.cargarMovimientos();
    this.cargarResumen();
  }

  cargarMovimientos() {
    const inicio = this.fechaInicio ? this.fechaInicio.toISOString() : undefined;
    const fin = this.fechaFin ? this.fechaFin.toISOString() : undefined;

    this.movimientoService.getAuditoria(inicio, fin).subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: () => this.toastr.error('Error al cargar auditoría', 'Error')
    });
  }

  cargarResumen() {
    this.movimientoService.getResumen().subscribe({
      next: (res) => {
        this.resumen = [
          { titulo: 'Inventario Inicial', cantidad: res.inventarioInicial, valor: 0, colorClass: 'color-azul' },
          { titulo: 'Entradas (Peso)', cantidad: res.entradas, valor: 0, colorClass: 'color-verde' },
          { titulo: 'Salidas (Peso)', cantidad: res.salidas, valor: 0, colorClass: 'color-rojo' },
          { titulo: 'Existencias', cantidad: res.existencias, valor: 0, colorClass: 'color-celeste' }
        ];
      },
      error: () => this.toastr.error('Error al cargar resumen', 'Error')
    });
  }

  aplicarRango() {
    this.cargarMovimientos();
  }

  limpiarRango() {
    this.fechaInicio = undefined;
    this.fechaFin = undefined;
    this.cargarMovimientos();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}