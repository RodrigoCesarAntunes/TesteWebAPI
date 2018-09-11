	-- Gerado por Oracle SQL Developer Data Modeler 18.2.0.179.0756
--   em:        2018-09-05 18:56:35 BRT
--   site:      SQL Server 2012
--   tipo:      SQL Server 2012



CREATE TABLE Cliente_Comercio (
    ID                 INTEGER identity(1,1) NOT NULL,
	Saldo_Em_Conta Decimal(14,2),
    Usuario_id         INTEGER NOT NULL,
    Usuario_Email   VARCHAR(100) NOT NULL
)

go 

CREATE unique nonclustered index Cliente_Comercio__idx ON Cliente_Comercio(Usuario_id,Usuario_Email) 
go

ALTER TABLE Cliente_Comercio ADD constraint Cliente_Comercio_pk PRIMARY KEY CLUSTERED (ID, Usuario_Email)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON )
 go

CREATE TABLE Cliente_Pessoa (
    ID                 INTEGER identity(1,1) NOT NULL,
    Usuario_id         INTEGER NOT NULL,
    Usuario_Email   VARCHAR(100) NOT NULL
)

go 

CREATE unique nonclustered index Cliente_Pessoa__idx ON Cliente_Pessoa(Usuario_ID,Usuario_Email) 
go

ALTER TABLE Cliente_Pessoa ADD constraint Cliente_Pessoa_pk PRIMARY KEY CLUSTERED (ID)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON ) 
	 go

CREATE TABLE Consulta 
    (
	Consulta_id             INTEGER NOT NULL IDENTITY,
	Data_Agendamento		datetime,
    Data_Hora               datetime,
    Is_Valida               bit,
    Cliente_comercio_id     INTEGER NOT NULL,
    Cliente_comercio_Email   VARCHAR(100) NOT NULL,
    Preco                   decimal(10,2),
    Pets_pet_id             INTEGER NOT NULL,
    IsCancelada             bit,
    Quemcancelou            VARCHAR(20),
    Motivo                  VARCHAR(250)
    ) 
	go

ALTER TABLE Consulta ADD constraint consulta_pk PRIMARY KEY CLUSTERED (Consulta_ID)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON ) 
	 go

CREATE TABLE Pagamento (
	ID_Pagamento         INTEGER NOT NULL identity(1,1),
    Numero_Cartao        VARCHAR(30),
    Proprietario_Cartao         VARCHAR(30),
    Data_Validade        DATE,
    CVC     VARCHAR(3),
    Usuario_id         INTEGER NOT NULL,
    Usuario_Email   VARCHAR(100) NOT NULL
)

go

ALTER TABLE Pagamento ADD constraint Pagamento_pk PRIMARY KEY CLUSTERED (ID_Pagamento)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON ) 
	 go

CREATE TABLE Pets (
    Pet_ID               INTEGER NOT NULL identity(1,1),
	Nome                 VARCHAR(100),
	What_pet             VARCHAR(50),
    Raca                VARCHAR(50),
    Peso              DECIMAL(6,3),
    Tamanho                 VARCHAR(20),
    Descricao          VARCHAR(250),
    Genero              varchar(1),
    Idade                  INTEGER,
    Cliente_pessoa_id    INTEGER NOT NULL,
    Cliente_pessoa_Email   VARCHAR(100) NOT NULL
)

go

ALTER TABLE Pets ADD constraint Pets_pk PRIMARY KEY CLUSTERED (Pet_ID)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON ) 
	 go


Create Table Pet_fotos
(
	ID int identity(1,1) primary key,
	FotoCaminho varchar(200),
	Pet_id INTEGER not null
	Foreign key (pet_id) references Pets(Pet_ID)
)

CREATE TABLE Services 
    (
     ID          INTEGER NOT NULL identity(1,1),
     Nome        VARCHAR(100),
     Descricao   VARCHAR(max) , 
     Preco DECIMAL (10,2) , 
     Cliente_Comercio_ID INTEGER NOT NULL , 
     Cliente_Comercio_Email VARCHAR (100) NOT NULL , 
     Consulta_Consulta_ID integer NOT NULL ) 
	 go

ALTER TABLE Services ADD constraint Services_pk PRIMARY KEY CLUSTERED (ID)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON ) 
	 go

CREATE TABLE Autenticacao
(
	ID int identity(1,1) unique,
	Senha char(256) not null,
	Email varchar(100) primary key not null,
	
)


CREATE TABLE Usuario (
	ID          INTEGER NOT NULL identity(1,1),
    Nome        VARCHAR(100),
    Email       VARCHAR(100) unique not null,
    Cpf_Cnpj    VARCHAR(100),
    Celular   VARCHAR(30),
    Idade         INTEGER,
    Endereco     VARCHAR(250),
    Cep         VARCHAR(20),
    Foto   VARCHAR(200),
	Data_cadastro datetime
	
	Foreign key (Email) references Autenticacao(email)
)

go


ALTER TABLE Usuario ADD constraint usuario_pk PRIMARY KEY CLUSTERED (ID, Email)
     WITH (
     ALLOW_PAGE_LOCKS = ON , 
     ALLOW_ROW_LOCKS = ON )
	  go

ALTER TABLE Cliente_Comercio
    ADD CONSTRAINT cliente_comercio_usuario_fk FOREIGN KEY ( usuario_id,usuario_Email ) REFERENCES usuario ( id,Email ) ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Cliente_Pessoa
    ADD CONSTRAINT cliente_pessoa_usuario_fk FOREIGN KEY ( usuario_id,Usuario_Email)
	REFERENCES usuario ( id,Email)ON DELETE No action ON UPDATE no action 
	go

ALTER TABLE Consulta
    ADD CONSTRAINT consulta_cliente_comercio_fk FOREIGN KEY ( cliente_comercio_id,
                                                              cliente_comercio_Email)
        REFERENCES cliente_comercio ( id,
                                      Usuario_Email)
            ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Consulta
    ADD CONSTRAINT consulta_pets_fk FOREIGN KEY ( pets_pet_id )
        REFERENCES pets ( pet_id )
            ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Pagamento
    ADD CONSTRAINT Pagamento_Usuario_fk FOREIGN KEY ( usuario_id,
                                                    usuario_Email)
        REFERENCES usuario ( id,
                             Email )
            ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Pets
    ADD CONSTRAINT pets_cliente_pessoa_fk FOREIGN KEY ( cliente_pessoa_id)
        REFERENCES cliente_pessoa (id)
            ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Services
    ADD CONSTRAINT services_cliente_comercio_fk FOREIGN KEY ( cliente_comercio_id,
                                                              cliente_comercio_Email)
        REFERENCES cliente_comercio ( id,
                                      Usuario_Email )
            ON DELETE CASCADE 
    ON UPDATE no action 
	go

ALTER TABLE Services
    ADD CONSTRAINT services_consulta_fk FOREIGN KEY ( consulta_consulta_id )
        REFERENCES consulta ( consulta_id )ON DELETE no action
    ON UPDATE no action 
	go



-- Relatório do Resumo do Oracle SQL Developer Data Modeler: 
-- 
-- CREATE TABLE                             7
-- CREATE INDEX                             2
-- ALTER TABLE                             15
-- CREATE VIEW                              0
-- ALTER VIEW                               0
-- CREATE PACKAGE                           0
-- CREATE PACKAGE BODY                      0
-- CREATE PROCEDURE                         0
-- CREATE FUNCTION                          0
-- CREATE TRIGGER                           0
-- ALTER TRIGGER                            0
-- CREATE DATABASE                          0
-- CREATE DEFAULT                           0
-- CREATE INDEX ON VIEW                     0
-- CREATE ROLLBACK SEGMENT                  0
-- CREATE ROLE                              0
-- CREATE RULE                              0
-- CREATE SCHEMA                            0
-- CREATE SEQUENCE                          0
-- CREATE PARTITION FUNCTION                0
-- CREATE PARTITION SCHEME                  0
-- 
-- DROP DATABASE                            0
-- 
-- ERRORS                                   1
-- WARNINGS                                 0
