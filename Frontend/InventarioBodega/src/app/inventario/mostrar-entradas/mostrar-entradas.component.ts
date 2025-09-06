import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { InventarioItemService } from '../../services/inventario-item.service';
import { InventarioItem } from '../../models/inventario-item.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-mostrar-entradas',
  templateUrl: './mostrar-entradas.component.html',
  styleUrls: ['./mostrar-entradas.component.css']
})
export class MostrarEntradasComponent implements OnInit {
  displayedColumns: string[] = ['referenciaPeso', 'fechaRegistroItem', 'pesoActual', 'estado'];
  dataSource = new MatTableDataSource<InventarioItem>([]);
  loading: boolean = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private inventarioItemService: InventarioItemService,
     private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.cargarEntradas();
  }

  cargarEntradas(): void {
    const idInventario = 1; 
    this.inventarioItemService.getByEntrada(idInventario).subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.loading = false;
      },
      error: () => {
        this.toastr.error('No se pudieron cargar las entradas', 'Error');
        this.loading = false;
      }
    });
  }

  // Filtro de la tabla
  aplicarFiltro(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}