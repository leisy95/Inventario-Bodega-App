import { Component, OnInit, ViewChild } from '@angular/core';
import { InventarioService, Inventario } from '../../services/inventario.service';
import { ToastrService } from 'ngx-toastr';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { EditarInventarioComponent } from '../editar-inventario/editar-inventario.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../shared/confirm-dialog/confirm-dialog.component';
import { RegistrarProductoComponent } from '../registrar-producto/registrar-producto.component';

@Component({
  selector: 'app-mostrar-inventario',
  templateUrl: './mostrar-inventario.component.html',
  styleUrls: ['./mostrar-inventario.component.css']
})
export class MostrarInventarioComponent implements OnInit {
  displayedColumns: string[] = [
    'referencia', 'tipoBolsa', 'tipoMaterial', 'densidad', 'color',
    'segundoColor', 'impresoNo', 'ancho', 'alto', 'calibre', 'peso',
    'cantidad', 'acciones'
  ];
  dataSource = new MatTableDataSource<Inventario>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private inventarioService: InventarioService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.cargarInventario();
  }

  cargarInventario() {
    this.inventarioService.getInventarios().subscribe({
      next: (data) => {

        // Ordenar por referencia (alfabético ascendente)
        const dataOrdenada = data.sort((a, b) =>
          a.referencia.localeCompare(b.referencia, 'es', { numeric: true })
        );

        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: () => this.toastr.error('Error al cargar inventario', 'Error')
    });
  }

  aplicarFiltro(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  ModalRegistrarProducto() {
    const dialogRef = this.dialog.open(RegistrarProductoComponent, {
      width: '900px',
       maxHeight: '90vh',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result: boolean | undefined) => {
      if (result === true) {
        this.toastr.success('Producto registrado', 'Éxito');
        this.cargarInventario(); // refresca tabla
      }
    });
  }

  editarInventario(item: Inventario) {
    (document.activeElement as HTMLElement)?.blur();

    const dialogRef = this.dialog.open(EditarInventarioComponent, {
      width: '600px',
      data: item
    });

    dialogRef.afterClosed().subscribe((result: boolean | undefined) => {
      if (result === true) {
        this.toastr.success('Inventario actualizado', 'Éxito');
        this.cargarInventario();
      }
    });
  }

  eliminarInventario(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: '¿Seguro que quieres eliminar esta referncia?' },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.inventarioService.deleteInventario(id).subscribe({
          next: () => {
            this.dataSource.data = this.dataSource.data.filter(i => i.id !== id);
            this.toastr.success('Inventario eliminado', 'Éxito');
          },
          error: () => this.toastr.error('No se pudo eliminar', 'Error')
        });
      }
    })
  }
}