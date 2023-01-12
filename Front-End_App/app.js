var container = document.getElementById("root");

//API'ye bağlanıp, eğer arama yapıldıysa arama sonuçlarını aksi halde bütün müşterilerin özlük bilgilerini getiren fonksiyon.
//Müşteri listesinin UI'i de burada yazılıyor. Sonda butonların eventListener'lar atanıyor.
async function getProfiles(search){

    var customersResponse;

    if(search){
        customersResponse = await fetch(`https://localhost:7139/customers/GetSearchedCustomers/${search}`)
    }
    else{
        customersResponse = await fetch(`https://localhost:7139/customers/GetCustomers`);
    }

    var customers = await customersResponse.json();

    
    container.innerHTML = `     
        <div class="row my-3 d-flex justify-content-between" style="padding-right:0px;">
            <div class="col-md-4 mb-2" style="padding-right:0px;">
                <button class="btn btn-primary w-100 getAll">Müşterileri Listele</button>
            </div>
            <div class="col-md-4 mb-2" style="padding-right:0px;">
                <button class="btn btn-primary w-100 add">Müşteri Ekle</button>
            </div>
            <div class="col-md-4" style="padding-right:0px;">
                <div class="input-group" style="padding-right:0px;">
                    <input type="text" class="form-control" placeholder="Ara..." id="search" />
                    <button class="btn btn-primary search">Ara</button>
                </div>
            </div>
        </div>
    `

    customers.forEach(customer => {


        container.innerHTML += `

            <div class="col-md-4">
                <div class="card border-2 border-secondary mb-2">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="card-subtitle small text-muted fw-bold">
                                    ${customer.isNameExtraordinary ? "Sıra Dışı":"Sıra Dışı Değil"}
                                </div>
                                <div class="card-subtitle small text-muted fw-bold">
                                    ${customer.tc}
                                </div>
                                <div class="card-title fw-bold">
                                    ${customer.name+" "+customer.surname}
                                </div>
                                ${customer.birthPlace+" "+customer.birthYear}
                            </div>
                            <div class="col-md-6">
                                <button id="${customer.id}" class="btn btn-warning w-100 mb-2 edit">Güncelle</button>
                                <button id="${customer.id}" class="btn btn-danger w-100 delete">Sil</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `
    });

    clearTimeout();

    setTimeout(() => {
        var editBtns = [...document.getElementsByClassName("edit")];
        var deleteBtns = [...document.getElementsByClassName("delete")];
        var addBtn = document.getElementsByClassName("add");
        var searchBtn = document.getElementsByClassName("search");
        var getAllBtn = document.getElementsByClassName("getAll");

        editBtns.forEach(element => {
            element.addEventListener("click", () => {
                updateClicked(element.id)
            })
        });

        deleteBtns.forEach(element => {
            element.addEventListener("click", () => {
                deleteClicked(element.id)
            })
        });

        [...addBtn][0].addEventListener("click", () => {
            addClicked()
        });

        
        [...searchBtn][0].addEventListener("click", () => {
            getProfiles(document.getElementById("search").value)
        });

        [...getAllBtn][0].addEventListener("click", () => {
            getProfiles()
        });

    }, 1000);
}

getProfiles();

//Müşteri ekleme butonu tıklanınca bu metod çalışır. Bu metodda Müşteri ekleme formu için UI hazırlanır.
async function addClicked(){
    
    container.innerHTML = `

        <h1>Müşteri Ekle</h1>

        <div class="row">
            <div class="col-md-8">

                <div class="row mb-3">
                    <label for="Name" class="col-sm-2 col-form-label">Müşteri Adı</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="Name">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="Surname" class="col-sm-2 col-form-label">Müşteri Soyadı</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="Surname">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="TC" class="col-sm-2 col-form-label">Müşteri TC</label>
                    <div class="col-sm-10">
                        <input type="number" class="form-control" id="TC">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="BirthPlace" class="col-sm-2 col-form-label">Müşteri Doğum Yeri</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="BirthPlace">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="BirthYear" class="col-sm-2 col-form-label">Müşteri Doğum Yılı</label>
                    <div class="col-sm-10">
                        <input type="number" class="form-control" id="BirthYear">
                    </div>
                </div>
    
                <button id="submitAddForm" class="btn btn-primary w-25">Ekle</button>
                <button id="cancelAddForm" class="btn btn-primary w-25">İptal</button>
    
            </div>
        </div>
    `
    clearTimeout();

    setTimeout(() => {
        var submitAddBtn = document.getElementById("submitAddForm");
        var cancelAddBtn = document.getElementById("cancelAddForm");
    
        submitAddBtn.addEventListener("click", () => {
            submitAddIsClicked()
        })

        cancelAddBtn.addEventListener("click", () => {
            container.innerHTML = "";
            getProfiles();
        })
    
    }, 1000);
}

//Müşteri ekleme formunda Ekle butonuna tıklanırsa bu fonksiyon çalışır. Form üzerindeki bilgiler okunur ve POST işlemi ile 
//RestAPI'ye gönderilir.
async function submitAddIsClicked(){

    var name = document.getElementById("Name").value
    var surname = document.getElementById("Surname").value
    var tc = document.getElementById("TC").value
    var birthPlace = document.getElementById("BirthPlace").value
    var birthYear = document.getElementById("BirthYear").value

    let postObje = {
        method: 'POST',
        body: JSON.stringify({
            name: name,
            surname: surname,
            tc: tc,
            birthPlace: birthPlace,
            birthYear: birthYear,
            contactInformations: [],
            addressInformations: []
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    }

    const customersUpdateResponse = await fetch(`https://localhost:7139/customers/AddCustomer`,postObje);

    console.log(customersUpdateResponse)

    if(customersUpdateResponse.status == 400){
        alert("Tc 11, doğum yılı 4 hane olmalı. Ayrıca hiçbir değer boş olmamalı. Şartlar sağlanmış durumda ise TC hali hazırda kayıtlıdır.");
    }

    container.innerHTML = "";
    getProfiles();
}

//Müşteri listesinde sil butonuna tıklanırsa bu fonksiyon çalışır. Kendisine gelen ID ile RestAPI'ye Delete isteği gönderir.
async function deleteClicked(id){

    let postObje = {
        method: 'DELETE',
    }

    const customersUpdateResponse = await fetch(`https://localhost:7139/customers/DeleteCustomer/${id}`,postObje);

    if(customersUpdateResponse.status == 204){
        alert("Silme İşlemi Başarılı")
        container.innerHTML = "";
        getProfiles();
    }
    else{
        alert("Silme İşlemi Başarısız")
    }

}

//Müşteri güncelleme butonu tıklanınca bu metod çalışır. Bu metodda öncelikle id'nin eşleştiği müşteri RestAPI'den alınır.
//Bu müşteri bilgilerinin value olarak kullanılacağı bir form oluşturlur. Böylece seçili müşteri bilgileri form üzerinde gözükür
async function updateClicked(id){

    console.log(id);
    const customersResponse = await fetch(`https://localhost:7139/customers/GetCustomer/${id}`);

    var customer = await customersResponse.json();

    container.innerHTML = `

        <h1>Müşteri Güncelle</h1>

        <div class="row">
            <div class="col-md-8">

                <input type="hidden" id="customerId" value="${customer.id}">

                <div class="row mb-3">
                    <label for="Name" class="col-sm-2 col-form-label">Müşteri Adı</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="Name" value="${customer.name}">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="Surname" class="col-sm-2 col-form-label">Müşteri Soyadı</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="Surname" value="${customer.surname}">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="TC" class="col-sm-2 col-form-label">Müşteri TC</label>
                    <div class="col-sm-10">
                        <input type="number" class="form-control" id="TC" value="${customer.tc}">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="BirthPlace" class="col-sm-2 col-form-label">Müşteri Doğum Yeri</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="BirthPlace" value="${customer.birthPlace}">
                    </div>
                </div>
                <div class="row mb-3">
                    <label for="BirthYear" class="col-sm-2 col-form-label">Müşteri Doğum Yılı</label>
                    <div class="col-sm-10">
                        <input type="number" class="form-control" id="BirthYear" value="${customer.birthYear}">
                    </div>
                </div>
    
                <button id="submitUpdateForm" class="btn btn-primary w-25">Güncelle</button>
                <button id="cancelUpdateForm" class="btn btn-primary w-25">İptal</button>
    
            </div>
        </div>
    `
    clearTimeout();

    setTimeout(() => {
        var submitUpdateBtn = document.getElementById("submitUpdateForm");
        var cancelUpdateBtn = document.getElementById("cancelUpdateForm");
    
        submitUpdateBtn.addEventListener("click", () => {
            submitUpdateIsClicked()
        })

        cancelUpdateBtn.addEventListener("click", () => {
            container.innerHTML = "";
            getProfiles();
        })
    
    }, 1000);
}

//Güncelleme formunda güncelle butonu tıklandığında çalışır. Formdaki bilgileri okur ve PUT request ile gönderir.
async function submitUpdateIsClicked(){

    var customerId = document.getElementById("customerId").value
    var name = document.getElementById("Name").value
    var surname = document.getElementById("Surname").value
    var tc = document.getElementById("TC").value
    var birthPlace = document.getElementById("BirthPlace").value
    var birthYear = document.getElementById("BirthYear").value

    let postObje = {
        method: 'PUT',
        body: JSON.stringify({
            id: customerId,
            name: name,
            surname: surname,
            tc: tc,
            birthPlace: birthPlace,
            birthYear: birthYear,
            contactInformations: [],
            addressInformations: []
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    }

    const customersUpdateResponse = await fetch(`https://localhost:7139/customers/PutCustomer`,postObje);

    if(customersUpdateResponse.status == 400){
        alert("Tc 11, doğum yılı 4 hane olmalı. Ayrıca hiçbir değer boş olmamalı. Şartlar sağlanmış durumda ise TC hali hazırda kayıtlıdır.");
    }

    container.innerHTML = "";
    getProfiles();
}

