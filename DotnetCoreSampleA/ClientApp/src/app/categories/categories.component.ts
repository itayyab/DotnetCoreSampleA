import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import $ from 'jquery';
import 'bootstrap';
import { Categories } from './Categories';
import { CategoriesService } from './categories.service';
import { ToastService } from '../_services/toast.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  heroes: Categories[];
  editHero: Categories; // the hero currently being edited
  title = '';
  name = '';
  id = 0;
  message = '';
  edit = false;
  @ViewChild('exampleModal', { static: false }) exampleModal: ElementRef;
  @ViewChild('namefiled', { static: false }) namefiled: ElementRef;
  @ViewChild('delbtn', { static: false }) delbtn: ElementRef;
  constructor(private heroesService: CategoriesService, @Inject('BASE_URL') baseUrl: string,public toastService: ToastService) {
   // console.log("BaeURL:"+baseUrl);
  }
  ngOnInit() {
    this.getHeroes();
  }

  getHeroes(): void {
    this.heroesService.getHeroes()
      .subscribe(heroes => (this.heroes = heroes));
  }
  open2() {
    this.edit = false;
    this.title = 'Add Category';
    this.name = '';
    this.id = 0;
    $(this.exampleModal.nativeElement).modal('show');
    //this.delbtn.nativeElement = false;
  }

  open(person: Categories) {
    this.edit = true;
    this.title = 'Edit Category';
    this.name = person.cat_name;
    //this.email = person.email;
    this.id = person.cat_id;
    $(this.exampleModal.nativeElement).modal('show');
    // this.delbtn.nativeElement = true;
    // const modalRef = this.modalService.open(ModalComponent);
    //  const modalRef = this.modalService.open(ModalComponent);
    //  modalRef.componentInstance.title = 'About';
    // modalRef.componentInstance.name = person.name;
    //  modalRef.componentInstance.id = person.id;
    //  this.announce();
  }



  update(person) {
    if (this.edit) {
      //      console.log(person);
      this.heroesService
        .updateHero(person)
        .subscribe(hero => {
          //   console.log(hero);
          this.closeModal();
          this.getHeroes();
          this.showSuccess(person.cat_name,'Category updated');
        });
    } 
  }
  closeModal() {
    $(this.exampleModal.nativeElement).modal('hide');
  }
  addX(name: string): void {
    name = name.trim();
    if (!name) { return; }
    this.heroesService.addHeroX({ cat_name: name } as Categories)
      .subscribe(hero => {
        //console.log(hero.email);
        this.heroes.push(hero);
        this.closeModal();
        this.showSuccess(name,'Category added');
      });
  }


  delete(hero: Categories): void {
   // console.log('size' + this.heroes.length);
    this.heroes = this.heroes.filter(h => h.cat_id !== hero.cat_id);
   // console.log('size' + this.heroes.length);
    // this.heroes.re
    this.heroesService
      .deleteHero(hero.cat_id)
      .subscribe();
    this.closeModal();
    this.showError(hero.cat_name);
    /*
    // oops ... subscribe() is missing so nothing happens
    this.heroesService.deleteHero(hero.id);
    */
  }

  //goToComponentB(people): void {
  //  this.router.navigate(['/peopledetails'], { state: { data: { people } } });
  //}
  showSuccess(product,header) {
    this.toastService.show(product, {
      classname: 'bg-success text-light',
      delay: 5000,
      autohide: true,
      headertext: header
    });
  }
  showError(product) {
    this.toastService.show(product, {
      classname: 'bg-danger text-light',
      delay: 2000,
      autohide: true,
      headertext: 'Category deleted'
    });
  }
}
