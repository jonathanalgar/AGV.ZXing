using OutSystems.Model.ExternalLibraries.SDK;
using System.Collections.Generic;
using MixERP.Net.VCards;
using MixERP.Net.VCards.Types;
using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Serializer;
using System;


namespace AGV.ZXing.Structures {

    [OSStructure(Description = "Defines a this to be shared as a QR code", OriginalName = "Contact")]
    public struct Contact {
        [OSStructureField(IsMandatory = true, Description = "Formated name", OriginalName = "FormatedName")]
        public string formatedName;

        [OSStructureField(Description = "Composed name", OriginalName = "ComposedName")]
        public Structures.ComposedName composedName;

        [OSStructureField(Description = "Organization", OriginalName = "Organization")]
        public string organization;

        [OSStructureField(Description = "Title", OriginalName = "Title")]
        public string title;

        [OSStructureField(DataType = OSDataType.PhoneNumber, Description = "Home phone number", OriginalName = "HomePhoneNumber")]
        public string homePhone;

        [OSStructureField(DataType = OSDataType.PhoneNumber, Description = "Work phone number", OriginalName = "WorkPhoneNumber")]
        public string workPhone;

        [OSStructureField(DataType = OSDataType.PhoneNumber, Description = "Mobile phone number", OriginalName = "MobilePhoneNumber")]
        public string mobilePhone;

        [OSStructureField(DataType = OSDataType.Email, Description = "Email", OriginalName = "Email")]
        public string email;

        [OSStructureField(Description = "Address", OriginalName = "Address")]
        public string address;

        [OSStructureField(Description = "Website", OriginalName = "Website")]
        public string website;

        [OSStructureField(Description = "Notes", OriginalName = "Notes")]
        public string notes;

        public Contact(string formatedName, ComposedName composedName, string organization, string title, 
                        string homePhone, string workPhone, string mobilePhone, string email, string address, string website, string notes):this() {
            this.formatedName = formatedName;
            this.composedName = new ComposedName(composedName);
            this.organization = organization;
            this.title = title;
            this.homePhone = homePhone;
            this.workPhone = workPhone;
            this.mobilePhone = mobilePhone;
            this.email = email;
            this.address = address;
            this.website = website;
            this.notes = notes;
        }

        public Contact(Contact c):this(){
            this.formatedName = c.formatedName;
            this.composedName = new ComposedName(c.composedName);
            this.organization = c.organization;
            this.title = c.title;
            this.homePhone = c.homePhone;
            this.workPhone = c.workPhone;
            this.mobilePhone = c.mobilePhone;
            this.email = c.email;
            this.address = c.address;
            this.website = c.website;
            this.notes = c.notes;
        }

        public string ToVCardString()
        {
            var vcard = new VCard {
                Version = VCardVersion.V3,
                FormattedName = this.formatedName,
                FirstName = this.composedName.firstName ?? "",
                LastName = this.composedName.lastName ?? "",
                MiddleName = this.composedName.middleNames ?? "",
                Organization = this.organization,
                Title = this.title,
                Telephones = new Telephone[] { 
                    new Telephone { Type = TelephoneType.Home, Number = this.homePhone },
                    new Telephone { Type = TelephoneType.Work, Number = this.workPhone },
                    new Telephone { Type = TelephoneType.Cell, Number = this.mobilePhone }
                },
                Url = new UriBuilder(this.website).Uri,
                Emails = new Email[] {
                    new Email { Type = EmailType.Smtp, EmailAddress = this.email }
                },
                Addresses = new Address[] {
                    new Address { Type = AddressType.Home, ExtendedAddress = this.address }
                },
                Note = this.notes
            };
            return VCardSerializer.Serialize(vcard);
        }

        public string ToMeCardString()
        {
            //MECARD:N:name;ORG:company;TEL:123;URL:http\://;EMAIL:email@mail.com;ADR:address address2;NOTE:memotitle;;
            return $"MECARD:N:{ this.formatedName.encodeQRCode() };TEL:{ this.homePhone.encodeQRCode() };" +
            $"URL:{ this.website.encodeQRCode() };EMAIL:{ this.email.encodeQRCode() };ADR:{ this.address.encodeQRCode() }" + 
            $"NOTE: { this.notes.encodeQRCode() };;";

        }
    }
}