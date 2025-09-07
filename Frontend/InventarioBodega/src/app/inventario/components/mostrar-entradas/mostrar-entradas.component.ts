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
    this.cargarEntradas();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  cargarEntradas(): void {
    this.inventarioItemService.getEntradas().subscribe({
      next: (data) => {
        this.dataSource.data = data;

        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: () => {
        this.toastr.error('No se pudieron cargar las entradas', 'Error');
      }
    });
  }

  editarItem(item: InventarioItem): void {
  const dialogRef = this.dialog.open(EditarEntradaPesoComponent, {
    width: '400px',
    data: { pesoActual: item.pesoActual }
  });

  dialogRef.afterClosed().subscribe(nuevoPeso => {
    if (nuevoPeso !== undefined && nuevoPeso !== null) {
      this.inventarioItemService.updatePesoEntrada(item.id, nuevoPeso).subscribe({
        next: (res) => {
          // actualizar tabla
          const idx = this.dataSource.data.findIndex(i => i.id === item.id);
          if (idx !== -1) {
            this.dataSource.data[idx].pesoActual = res.pesoActual ?? nuevoPeso;
            this.dataSource.data[idx].referenciaPeso = res.referenciaPeso ?? this.dataSource.data[idx].referenciaPeso;
            this.dataSource._updateChangeSubscription();
          }
          this.toastr.success('Peso actualizado correctamente', 'Éxito');
        },
        error: () => this.toastr.error('No se pudo actualizar el peso', 'Error')
      });
    }
  });
}

  eliminarItem(id: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: '¿Seguro que quieres eliminar esta entrada?' },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.inventarioItemService.deleteEntrada(id).subscribe({
          next: () => {
            // Eliminar del dataSource
            this.dataSource.data = this.dataSource.data.filter(item => item.id !== id);
            this.toastr.success('Inventario eliminado', 'Éxito');
          },
          error: () => this.toastr.error('No se pudo eliminar')
        });
      }
    });
  }

  aplicarFiltro(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}