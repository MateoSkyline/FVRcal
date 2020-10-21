import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyemailRegisterComponent } from './verifyemail-register.component';

describe('VerifyemailRegisterComponent', () => {
  let component: VerifyemailRegisterComponent;
  let fixture: ComponentFixture<VerifyemailRegisterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerifyemailRegisterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyemailRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
