import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarEntradaPesoComponent } from './registrar-entrada-peso.component';

describe('MostrarVistaComponent', () => {
  let component: RegistrarEntradaPesoComponent;
  let fixture: ComponentFixture<RegistrarEntradaPesoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegistrarEntradaPesoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrarEntradaPesoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
