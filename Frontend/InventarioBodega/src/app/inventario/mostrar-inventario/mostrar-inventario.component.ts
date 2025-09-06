import { Component, OnInit, ViewChild } from '@angular/core';
import { InventarioService, Inventario } from '../../services/inventario.service';
import { ToastrService } from 'ngx-toastr';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

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
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.inventarioService.getInventarios().subscribe({
      next: (data) => {
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

  eliminarInventario(id: number) {
    if (confirm('¿Seguro que deseas eliminar este inventario?')) {
      this.inventarioService.deleteInventario(id).subscribe({
        next: () => {
          this.dataSource.data = this.dataSource.data.filter(i => i.id !== id);
          this.toastr.success('Inventario eliminado', 'Éxito');
        },
        error: () => this.toastr.error('No se pudo eliminar', 'Error')
      });
    }
  }
}