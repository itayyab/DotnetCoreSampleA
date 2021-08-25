import { AfterViewChecked, ChangeDetectorRef, Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { Product } from './Product';
import $ from 'jquery';
import 'bootstrap';
import { ProductsService } from './products.service';
import { CategoriesService } from '../categories/categories.service';
import { Categories } from '../categories/Categories';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, AfterViewChecked {

  file: File = null; // Variable to store file
  products: Product[];
  categories: Categories[];
  editHero: Product; // the hero currently being edited
  title = '';
  desc = '';
  price = 0;
  name = '';
  id = 0;
  message = '';
  cat = 0;
  pic = '';
  edit = false;
  loginForm: FormGroup;
  submitted = false;
  loading = false;


  @ViewChild('exampleModal', { static: false }) exampleModal: ElementRef;
  @ViewChild('toast', { static: false }) toast: ElementRef;
  @ViewChild('namefiled', { static: false }) namefiled: ElementRef;
  @ViewChild('descfiled', { static: false }) descfiled: ElementRef;
  @ViewChild('pricefiled', { static: false }) pricefiled: ElementRef;
  @ViewChild('selectfield', { static: false }) selectfield: ElementRef;
  @ViewChild('delbtn', { static: false }) delbtn: ElementRef;
  @ViewChild('filefield', { static: false }) filefield: ElementRef;
 

  onChange(event) {
    this.file = event.target.files[0];
  }

  // OnClick of button Upload
  onUpload() {
    
   
  /*  this.heroesService.upload(this.file).subscribe(
      (event: any) => {
        if (typeof (event) === 'object') {

          
        }
      }
    );*/
  }



  /*fileToUpload: File | null = null;
  handleFileInput(files: FileList) {
    this.fileToUpload = files.item[0];
    console.log("file:" + this.fileToUpload.name);
    this.uploadFileToActivity();
  }
  uploadFileToActivity() {
    this.heroesService.postFile(this.fileToUpload).subscribe(data => {
      // do something, if upload success
      console.log("fileiuploaded");
    }, error => {
      console.log(error);
    });
  }*/
  constructor(private heroesService: ProductsService, private catService: CategoriesService, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder, private readonly changeDetectorRef: ChangeDetectorRef) {
    console.log("BaeURL:" + baseUrl);
  }
  ngAfterViewChecked(): void {
    this.changeDetectorRef.detectChanges();
  }
  ngOnInit() {
    this.getProdcuts();
   
    this.getHeroes();
    this.loginForm = this.formBuilder.group({
      price: ['', Validators.required],
      name: ['', Validators.required],
      desc: ['', Validators.required]
    });
  }

  getProdcuts(): void {
    this.heroesService.getHeroes()
      .subscribe(heroes => {
        (this.products = heroes);
       // console.log(JSON.stringify(this.products));
      });
  }
  getHeroes(): void {
    this.catService.getHeroes()
      .subscribe(heroes => (this.categories = heroes));
  }
  open2() {
    this.edit = false;
    this.title = 'Add Product';
    this.name = '';
    this.id = 0;
    this.name = '';
    this.id = 0;
    this.desc = '';
    this.price = 0;
    this.message = '';
    this.file = null;

    $(this.exampleModal.nativeElement).modal('show');
    //this.delbtn.nativeElement = false;
  }
  closeModal() {
    $(this.exampleModal.nativeElement).modal('hide');

    this.name = '';
    this.id = 0;
    this.title = '';
    this.desc = '';
    this.price = 0;
    this.message = '';
    this.filefield.nativeElement.value = "";

    this.file = null;
  }
 
  addX(pr: Product) {

    //console.log(JSON.stringify(pr));
    this.submitted = true;
    if (this.loginForm.invalid) {
      console.log("invalid");
      return;
    }
    if (this.file != null && this.file != undefined) {
     
      this.heroesService
        .postFile(this.file)
        .subscribe(hero => {
          //console.log(hero);
          var catname = this.categories.find(x => x.cat_id == pr.category.cat_id);
          const product: Product = {
            pr_id: pr.pr_id, pr_name: pr.pr_name, pr_desc: pr.pr_desc, pr_Picture: hero, pr_price: parseInt(pr.pr_price.toString()), categoryForeignKey: parseInt(pr.category.cat_id.toString()), category: null
          };
          console.log(JSON.stringify(product));
          this.heroesService.addHeroX(product)
            .subscribe(hero => {
              //console.log(hero.email);
              //  console.log("DAta:" + JSON.stringify(hero));
              this.products.push(hero);
              $(this.exampleModal.nativeElement).modal('hide');
              this.message = 'Product details added';
              $(this.toast.nativeElement).toast('show');
            });
          //  console.log("Fileuploaded" + hero);
          //  return hero;
        });
    } else {
      console.log(this.file); 
      var catname = this.categories.find(x => x.cat_id == pr.category.cat_id);
      const product: Product = {
        pr_id: pr.pr_id, pr_name: pr.pr_name, pr_desc: pr.pr_desc, pr_Picture: "StaticFiles/Images/no_image.png", pr_price: parseInt(pr.pr_price.toString()), categoryForeignKey: parseInt(pr.category.cat_id.toString()), category: null
          };
          this.heroesService.addHeroX(product)
            .subscribe(hero => {
              //console.log(hero.email);
              //  console.log("DAta:" + JSON.stringify(hero));
              this.products.push(hero);
              $(this.exampleModal.nativeElement).modal('hide');
              this.message = 'Product details added';
              $(this.toast.nativeElement).toast('show');
            });
          //  console.log("Fileuploaded" + hero);
          //  return hero;
    }
    this.closeModal();


    


  //
   

    /*name = name.trim();
    if (!name) { return; }
    this.heroesService.addHeroX({ cat_name: name } as Categories)
      .subscribe(hero => {
        //console.log(hero.email);
        this.heroes.push(hero);
        $(this.exampleModal.nativeElement).modal('hide');
        this.message = 'Category details added';
        $(this.toast.nativeElement).toast('show');
      });*/
  }
  open(person: Product) {
    console.log(JSON.stringify(person));
    this.edit = true;
    this.title = 'Edit Product';
    this.name = person.pr_name;
    //this.email = person.email;
    this.pic = person.pr_Picture;
    this.id = person.pr_id;
    this.desc = person.pr_desc;
    this.price = person.pr_price;
    this.cat = person.category.cat_id;
    this.descfiled.nativeElement.Value = person.pr_desc;
    

    $(this.exampleModal.nativeElement).modal('show');
    // this.delbtn.nativeElement = true;
    // const modalRef = this.modalService.open(ModalComponent);
    //  const modalRef = this.modalService.open(ModalComponent);
    //  modalRef.componentInstance.title = 'About';
    // modalRef.componentInstance.name = person.name;
    //  modalRef.componentInstance.id = person.id;
    //  this.announce();
  }
  selectChange() {

  }
  update(pr: Product) {

    console.log("RX:"+JSON.stringify(pr));
    this.submitted = true;
    if (this.edit) {
      //      console.log(person);
      
      if (this.loginForm.invalid) {
        console.log("invalid");
        return;
      }
      if (this.file != null && this.file != undefined) {
        //  console.log(this.file);
          this.heroesService
            .postFile(this.file)
            .subscribe(hero => {
             // console.log(hero);
              var catname = this.categories.find(x => x.cat_id == pr.category.cat_id);
              const product: Product = {
                pr_id: pr.pr_id, pr_name: pr.pr_name, pr_desc: pr.pr_desc, pr_Picture: hero, pr_price: parseInt(pr.pr_price.toString()), categoryForeignKey: parseInt(pr.category.cat_id.toString()), category: null
              };
              this.heroesService.updateHero(product)
                .subscribe(hero => {
                  //console.log(hero.email);
                  //  console.log("DAta:" + JSON.stringify(hero));
                  this.getProdcuts();
                  $(this.exampleModal.nativeElement).modal('hide');
                  this.message = 'Product details updated';
                  $(this.toast.nativeElement).toast('show');
                });
              //  console.log("Fileuploaded" + hero);
              //  return hero;
            });
        }else {
         // console.log(this.file);
        var catname = this.categories.find(x => x.cat_id == pr.category.cat_id);
        const product: Product = {
          pr_id: pr.pr_id, pr_name: pr.pr_name, pr_desc: pr.pr_desc, pr_Picture: this.pic, pr_price: parseInt(pr.pr_price.toString()), categoryForeignKey: parseInt(pr.category.cat_id.toString()), category: null
        };
        console.log("RX2:" + JSON.stringify(pr));
        this.heroesService.updateHero(product)
                .subscribe(hero => {
                  //console.log(hero.email);
                  //  console.log("DAta:" + JSON.stringify(hero));
                  this.getProdcuts();
                  $(this.exampleModal.nativeElement).modal('hide');
                  this.message = 'Product details updated';
                  $(this.toast.nativeElement).toast('show');
                });
              //  console.log("Fileuploaded" + hero);
              //  return hero;
      }
      this.closeModal();
      //this.loading = true;
     // console.log(this.file + JSON.stringify(person));
     // const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
      /*this.authService.login(this.loginForm.value)
        .subscribe(
          () => {
            this.router.navigate([returnUrl]);
          },
          () => {
            this.loading = false;
            this.loginForm.reset();
            this.loginForm.setErrors({
              invalidLogin: true
            });
          });*/
    /*  this.heroesService
        .updateHero(person)
        .subscribe(hero => {
          //   console.log(hero);
          $(this.exampleModal.nativeElement).modal('hide');
          this.getHeroes();
          this.message = 'Product details updated';
          $(this.toast.nativeElement).toast('show');

          //  this.sendMessageToParent('messageToSendC');
          // replace the hero in the heroes list with update from server
          // const ix = hero ? this.person.findIndex(h => h.id === hero.id) : -1;
          // if (ix > -1) {
          //   this.person[ix] = hero;/   }
        });*/
      // this.editHero = undefined;
      // }
    } else {
     // this.addX(person);
    }


  }
  get loginFormControl() { return this.loginForm.controls; }
  delete(hero: Product): void {
    console.log('size' + this.products.length);
    this.products = this.products.filter(h => h.pr_id !== hero.pr_id);
    console.log('size' + this.products.length);
    // this.heroes.re
    this.heroesService
      .deleteHero(hero.pr_id)
      .subscribe();
    $(this.exampleModal.nativeElement).modal('hide');
    // this.getHeroes();
    this.message = 'Product deleted';
    $(this.toast.nativeElement).toast('show');
    /*
    // oops ... subscribe() is missing so nothing happens
    this.heroesService.deleteHero(hero.id);
    */
    this.closeModal();
  }
  public getCat(catid: number): string {
    console.log("CAT:"+catid);
    var catname = this.categories.find(x => x.cat_id == catid).cat_name;
    // var cat: Categories = { cat_id: catid, cat_name: catname };
    return catname;
  }
   
  public createImgPath = (serverPath: string) => {
    return 'https://localhost:44316/'+serverPath;
  }
}
