import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { InventarioItemService } from '../../services/inventario-item.service';
import { InventarioItem } from '../../models/inventario-item.model';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../shared/confirm-dialog/confirm-dialog.component';
import { EditarEntradaPesoComponent } from '../editar-entrada-peso/editar-entrada-peso.component';

@Component({
  selector: 'app-mostrar-entradas',
  templateUrl: './mostrar-entradas.component.html',
  styleUrls: ['./mostrar-entradas.component.css']
})
export class MostrarEntradasComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['referenciaPeso', 'fechaRegistroItem', 'pesoActual', 'estado', 'acciones'];
  dataSource = new MatTableDataSource<InventarioItem>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private inventarioItemService: InventarioItemService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    // Configurar filtro personalizado para solo referenciaPeso
    this.dataSource.filterPredicate = (data: InventarioItem, filter: string) =>
      data.referenciaPeso.toLowerCase().includes(filter);

    this.cargarEntradas();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  cargarEntradas(): void {
    this.inventarioItemService.getEntradas().subscribe({
      next: (data: InventarioItem[]) => {
        this.dataSource.data = data;
      },
      error: () => this.toastr.error('No se pudieron cargar las entradas', 'Error')
    });
  }

  aplicarFiltro(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.dataSource.filter = filterValue;

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  eliminarItem(id: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: '¿Seguro que quieres eliminar esta entrada?' },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(confirm => {
      if (confirm) {
        this.inventarioItemService.eliminarEntrada(id).subscribe({
          next: (res) => {
            this.cargarEntradas(); // refrescar la tabla
            this.toastr.success(res.message || 'Entrada eliminada correctamente', 'Éxito');
          },
          error: (err) => {
            this.toastr.error(err.error || 'Error al eliminar la entrada', 'Error');
          }
        });
      }
    });
  }
}