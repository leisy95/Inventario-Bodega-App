import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditoriaInventarioComponent } from './auditoria-inventario.component';

describe('AuditoriaInventarioComponent', () => {
  let component: AuditoriaInventarioComponent;
  let fixture: ComponentFixture<AuditoriaInventarioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuditoriaInventarioComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuditoriaInventarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
