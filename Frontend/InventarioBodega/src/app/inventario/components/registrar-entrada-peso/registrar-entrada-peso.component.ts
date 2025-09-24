import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Inventario, InventarioService } from '../../services/inventario.service';
import { FormControl } from '@angular/forms';
import { debounceTime, map, Observable, startWith } from 'rxjs';
import { ConfirmDialogComponent } from '../shared/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registrar-entrada-peso',
  templateUrl: './registrar-entrada-peso.component.html',
  styleUrls: ['./registrar-entrada-peso.component.css']
})
export class RegistrarEntradaPesoComponent implements OnInit {
  referenciaControl = new FormControl('');
  referencias: string[] = [];
  referenciasFiltradas!: Observable<string[]>;
  pesoIngresado: number | null = null;
  inventario: Inventario | null = null;
  pesoActual: number | null = null;
  pesoMostrado: number | null = null;
  fechaImpresion: Date = new Date();
  ultimaReferenciaPeso: string | null = null;

  constructor(
    private inventarioService: InventarioService,
    private dialog: MatDialog,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    // Trae todas las referencias al inicio
    this.filtrarReferencias();
    this.configurarEventoImpresion();
  }

  ngOnDestroy(): void {
    this.removerEventoImpresion(); // 
  }

  //Fecha de Impresion
  actualizarFechaImpresion(): void {
    this.fechaImpresion = new Date();
  }

  private beforePrintListener = () => {
    this.actualizarFechaImpresion();
  };

  private configurarEventoImpresion(): void {
    window.addEventListener('beforeprint', this.beforePrintListener);
  }

  private removerEventoImpresion(): void {
    window.removeEventListener('beforeprint', this.beforePrintListener);
  }

  filtrarReferencias() {
    this.inventarioService.getInventarios().subscribe({
      next: (res) => {
        // Lista ordenada alfabéticamente UNA SOLA VEZ
        this.referencias = res
          .map(i => i.referencia)
          .sort((a, b) => a.localeCompare(b, 'es', { sensitivity: 'base' }));

        // Filtrado en memoria con debounce
        this.referenciasFiltradas = this.referenciaControl.valueChanges.pipe(
          startWith(''),
          debounceTime(200),
          map(value => this._filter(value || ''))
        );
      }
    });
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    // Filtra por cualquier parte de la referencia
    return this.referencias.filter(ref => ref.toLowerCase().includes(filterValue));
  }

  buscar() {
    this.pesoActual = null;

    const ref = this.referenciaControl.value;
    if (!ref || !ref.trim()) return;

    this.inventarioService.getByReferencia(ref).subscribe({
      next: (res) => {
        this.inventario = res;
        this.pesoIngresado = null;
      },
      error: () => {
        this.inventario = null;
      }
    });
  }

  @ViewChild('segundoInput') segundoInput!: ElementRef<HTMLInputElement>;
  @ViewChild('referenciaInput') referenciaInput!: ElementRef<HTMLInputElement>;


  moverAlSegundo() {
    if (this.segundoInput) {
      this.segundoInput.nativeElement.focus();
    }
  }

  confirmarIngresoPeso() {
    const peso = Number(this.pesoIngresado);

    // Validar si es inválido o menor o igual a 0
    if (!peso || peso <= 0) {
      this.toastr.warning('El peso debe ser mayor a 0', 'Atención');
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        title: 'Confirmar Ingreso de Peso',
        message: `¿Está seguro de ingresar el peso de ${peso} kg para la referencia ${this.referenciaControl.value}?`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.agregarPeso(peso);
      }
    });
  }

  agregarPeso(peso: number) {
    if (!peso) return;

    if (this.referenciaControl.value) {
      this.inventarioService.ingresarPeso(this.referenciaControl.value, peso).subscribe({
        next: (response) => {
          this.pesoActual = response.peso;
          this.ultimaReferenciaPeso = response.referenciaPeso;
          this.pesoMostrado = peso;

          this.toastr.success('Peso ingresado correctamente', 'Éxito');
          this.pesoIngresado = null;

          // Regresar al input de referencia
          setTimeout(() => {
            if (this.referenciaInput) {
              this.referenciaInput.nativeElement.focus();
              this.referenciaInput.nativeElement.select();
            }
          }, 300);
        },
        error: (err) => {
          console.error('Error al guardar peso:', err);
          this.toastr.error('No se pudo guardar el peso', 'Error');
        }
      });
    } else {
      this.toastr.warning('Debe seleccionar una referencia antes de guardar', 'Atención');
    }
  }

  /* Imprimir etiqueta */
  imprimirEtiqueta() {
    this.actualizarFechaImpresion();
    window.print();
  }

  getReferenciaParaBarcode(): string {
    return this.ultimaReferenciaPeso
      ? String(this.ultimaReferenciaPeso).toUpperCase()
      : '';
  }
}