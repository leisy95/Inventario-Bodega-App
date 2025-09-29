import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MovimientoInventarioService } from '../../services/movimiento-inventario.service';

@Component({
  selector: 'app-auditoria-inventario',
  templateUrl: './auditoria-inventario.component.html',
  styleUrls: ['./auditoria-inventario.component.css']
})
export class AuditoriaInventarioComponent implements OnInit {

  resumenInventario: any[] = [];
  totalesPorTipo: any[] = [];
  referenciasMasMovidas: any[] = [];
  usuariosMasActivos: any[] = [];

  dataSource = new MatTableDataSource<any>([]);
  displayedColumns: string[] = ['referencia', 'tipo', 'peso', 'fecha', 'usuario'];

  fechaInicio: Date | null = null;
  fechaFin: Date | null = null;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private movimientoService: MovimientoInventarioService) { }

  ngOnInit(): void {
    this.cargarAuditoria();
  }

  // Formatea fecha a 'YYYY-MM-DD' para enviar al backend
  private formatDate(date: Date | null): string | undefined {
    if (!date) return undefined;
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  // =========================
  // Cargar todo: tabla, resumen y reportes
  // =========================
  cargarAuditoria(): void {
    const inicio = this.formatDate(this.fechaInicio);
    const fin = this.formatDate(this.fechaFin);

    this.movimientoService.getAuditoria(inicio, fin).subscribe((data: any) => {
      // 1️⃣ Resumen de inventario
      const resumenData = data.resumen;
      this.resumenInventario = [
        { titulo: 'Total Referencias', cantidad: resumenData.inventarioInicial?.cantidad || 0, colorClass: 'color-gris' },
        { titulo: 'Entradas-Peso', cantidad: resumenData.entradas?.cantidad || 0, peso: resumenData.entradas?.peso || 0, colorClass: 'color-verde' },
        { titulo: 'Salidas-Peso', cantidad: resumenData.salidas?.cantidad || 0, peso: resumenData.salidas?.peso || 0, colorClass: 'color-rojo' },
        { titulo: 'Existencias', cantidad: resumenData.existencias?.cantidad || 0, peso: resumenData.existencias?.peso || 0, colorClass: 'color-azul' },
      ];

      // 2️⃣ Totales por tipo
      this.totalesPorTipo = data.totalesPorTipo || [];

      // 3️⃣ Referencias más movidas
      this.referenciasMasMovidas = data.referenciasMasMovidas || [];

      // 4️⃣ Usuarios más activos
      this.usuariosMasActivos = data.usuariosMasActivos || [];

      // 5️⃣ Tabla de movimientos
      this.dataSource = new MatTableDataSource(data.movimientos || []);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  // =========================
  // Aplicar rango de fechas
  // =========================
  aplicarRango(): void {
    this.cargarAuditoria();
  }

  // =========================
  // Limpiar rango de fechas
  // =========================
  limpiarRango(): void {
    this.fechaInicio = null;
    this.fechaFin = null;
    this.cargarAuditoria();
  }

  // =========================
  // Filtrar tabla por texto
  // =========================
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  // =========================
  // Filtrado automático al cambiar rango de fechas
  // =========================
  rangoCambiado(event: any): void {
    if (this.fechaInicio && this.fechaFin) {
      this.cargarAuditoria();
    }
  }
}