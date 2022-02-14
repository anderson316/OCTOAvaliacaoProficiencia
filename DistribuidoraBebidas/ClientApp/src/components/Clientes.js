import React, { Component } from 'react';
var paginaMax = 1;
var podeVoltar = true;
var podePassar = true;
var clientesPag = [];
var self = null;
var dataCompleta = [];

export class Clientes extends Component {
    static displayName = Clientes.name;

    constructor(props) {
        super(props);
        this.state = { clientes: [], loading: true, paginaAtual: 1, buscando: false};
    }

    componentDidMount() {
        this.GetPages();
        this.populateClienteData();
    }

    componentDidUpdate()
    {
        if (this.state.clientes.length === 0 && this.state.buscando === false) {
            this.GetPages();
            this.populateClienteData();
        }
    }
    
    static renderClientes(clientes) {
        return (
            <>
            <div class="input-group mb-3">
                <input type="text" id="busca" class="form-control" placeholder="Busca...">
                </input>
                <div class="input-group-append">
                    <button class="btn btn-primary" type="button" onClick={() => self.buscarNome(document.getElementById("busca").value)}>Buscar</button>
                </div>
            </div>
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>
                            <button type="button" class="btn btn-primary btn-lg btn-block" onClick={() => self.organizarNome()}>
                            Nome
                            </button>
                        </th>
                        <th>
                            <button type="button" class="btn btn-primary btn-lg btn-block" onClick={() => self.organizarCPF()}>
                            CPF
                            </button>
                        </th>
                        <th>
                            <button type="button" class="btn btn-primary btn-lg btn-block" onClick={() => self.organizarCNPJ()}>
                            CNPJ
                            </button>
                        </th>
                        <th>
                            <button type="button" class="btn btn-primary btn-lg btn-block" onClick={() => self.organizarEmail()}>
                            E-Mail
                            </button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {clientes.map(cliente =>
                        <tr key={cliente.clienteID}>
                            <td>{cliente.nome}</td>
                            <td>{cliente.cpf}</td>
                            <td>{cliente.cnpj}</td>
                            <td>{cliente.email}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            </>
        );
    }

    async buscarNome(nome){
        const response = await fetch('https://localhost:44388/api/Clientes?nome='+nome, {
        });
        const pageResponse = await fetch('https://localhost:44388/api/Clientes/pages?nome='+nome, {
            headers: {
                'pagina':self.state.paginaAtual
            }});
        const data = await response.json();
        const dataPagina = await pageResponse.json();
        dataCompleta = data;
        paginaMax = dataPagina.numeroPaginas;
        podePassar = dataPagina.proximaPagina;
        podeVoltar = dataPagina.podeVoltar;
        if(self.state.paginaAtual === 1){
            clientesPag = data.slice(self.state.paginaAtual - 1, self.state.paginaAtual * 20);
        }
        else{
            clientesPag = data.slice(self.state.paginaAtual * 10, self.state.paginaAtual * 20);
        }
        this.setState({ clientes: clientesPag, loading: false, buscando: true });
    }


    organizarNome() {
        var organizar = dataCompleta.sort((a, b) => a.nome.localeCompare(b.nome));
        if(self.state.paginaAtual === 1){
            clientesPag = organizar.slice(self.state.paginaAtual - 1, self.state.paginaAtual * 20);
        }
        else{
            clientesPag = organizar.slice(self.state.paginaAtual * 10, self.state.paginaAtual * 20);
        }   
        this.setState({ clientes: clientesPag, loading: false });
    }

    organizarCPF() {
        var organizar = dataCompleta.sort((a, b) => a.cpf.localeCompare(b.cpf));
        if(self.state.paginaAtual === 1){
            clientesPag = organizar.slice(self.state.paginaAtual - 1, self.state.paginaAtual * 20);
        }
        else{
            clientesPag = organizar.slice(self.state.paginaAtual * 10, self.state.paginaAtual * 20);
        }   
        this.setState({ clientes: clientesPag, loading: false });
    }

    organizarCNPJ() {
        var organizar = dataCompleta.sort((a, b) => a.cnpj.localeCompare(b.cnpj));
        if(self.state.paginaAtual === 1){
            clientesPag = organizar.slice(self.state.paginaAtual - 1, self.state.paginaAtual * 20);
        }
        else{
            clientesPag = organizar.slice(self.state.paginaAtual * 10, self.state.paginaAtual * 20);
        }   
        this.setState({ clientes: clientesPag, loading: false });
    }

    organizarEmail() {
        var organizar = dataCompleta.sort((a, b) => a.email.localeCompare(b.email));
        if(self.state.paginaAtual === 1){
            clientesPag = organizar.slice(self.state.paginaAtual - 1, self.state.paginaAtual * 20);
        }
        else{
            clientesPag = organizar.slice(self.state.paginaAtual * 10, self.state.paginaAtual * 20);
        }   
        this.setState({ clientes: clientesPag, loading: false });
    }

    render() {
        self = this;
        let contents = this.state.loading
            ? <p><em>Carregado...</em></p>
            : Clientes.renderClientes(this.state.clientes);

        return (
            <>
            <div>
                <h1 id="tabelLabel" >CLIENTES</h1>
                {contents}
            </div>
            <div className="pagination">
                <button onClick={() => this.primeiraPagina(this)} disabled={!podeVoltar}>
                  {'<<'}
                </button>{' '}
                <button onClick={() => this.voltarPagina(this)} disabled={!podeVoltar}>
                  {'<'}
                </button>{' '}
                <button onClick={() => this.passarPagina(this)} disabled={!podePassar}>
                    {'>'}
                </button>{' '}
                <button onClick={() => this.ultimaPagina(this)} disabled={!podePassar}>
                  {'>>'}
                </button>{' '}
                <span>
                  Pagina{' '}
                  <strong>
                    {this.state.paginaAtual} de {paginaMax}
                  </strong>{' '}
                </span>
            </div>
            </>
        );
    }

    async GetPages() {
        const response = await fetch('https://localhost:44388/api/Clientes/pages', {
            headers: {
                'pagina':this.state.paginaAtual
            }});
        const data = await response.json();
        paginaMax = data.numeroPaginas;
        podePassar = data.proximaPagina;
        podeVoltar = data.podeVoltar;
    }

    async populateClienteData() {
        const response = await fetch('https://localhost:44388/api/Clientes', {
        });
        const data = await response.json();
        dataCompleta = data;
        if(this.state.paginaAtual === 1){
            clientesPag = data.slice(this.state.paginaAtual - 1, this.state.paginaAtual * 20);
        }
        else{
            clientesPag = data.slice(this.state.paginaAtual * 10, this.state.paginaAtual * 20);
        }
        this.setState({ clientes: clientesPag, loading: false, buscando: false });
    }

    primeiraPagina(cliente)
    {
        cliente.setState({clientes: [], loading: false, paginaAtual: 1});
    }

    passarPagina(cliente)
    {
        cliente.setState({clientes: [], loading: false, paginaAtual: cliente.state.paginaAtual + 1});
    }

    voltarPagina(cliente)
    {
        cliente.setState({clientes: [], loading: false, paginaAtual: cliente.state.paginaAtual - 1});
    }
    ultimaPagina(cliente)
    {
        cliente.setState({clientes: [], loading: false, paginaAtual: paginaMax});
    }
}
