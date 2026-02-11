<p align="center">
  <img src="https://img.shields.io/badge/Plataforma-Windows-0078D6?logo=windows&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET_Framework-4.5.2+-512BD4?logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Idioma-Portugu%C3%AAs_BR-009739?logo=googletranslate&logoColor=white" />
  <img src="https://img.shields.io/badge/Active_Directory-Suportado-orange?logo=microsoft&logoColor=white" />
  <img src="https://img.shields.io/github/license/ManoAlee/GPO-Plus?color=blue" />
</p>

# 🛡️ GPO Plus

**Editor avançado de Políticas de Grupo para Windows — com suporte a Active Directory e interface 100% em Português Brasileiro.**

Fork aprimorado do projeto [Policy Plus](https://github.com/Fleex255/PolicyPlus) por [Ben Nordick (Fleex255)](https://github.com/Fleex255), com adição de gerenciamento de GPOs de domínio via LDAP e tradução completa para PT-BR.

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Novidades desta Versão](#-novidades-desta-versão)
- [Capturas de Tela](#-capturas-de-tela)
- [Instalação](#-instalação)
- [Como Usar](#-como-usar)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura do Projeto](#-arquitetura-do-projeto)
- [Requisitos do Sistema](#-requisitos-do-sistema)
- [Compilação](#-compilação)
- [Contribuindo](#-contribuindo)
- [Créditos e Referências](#-créditos-e-referências)
- [Licença](#-licença)

---

## 🎯 Visão Geral

O **GPO Plus** é um editor de Políticas de Grupo que funciona em **todas as edições do Windows**, incluindo Home e Starter — edições que não possuem o `gpedit.msc` nativo.

### O que diferencia este fork:

| Recurso | Policy Plus (Original) | GPO Plus (Este Fork) |
|---------|:---------------------:|:-------------------:|
| Edição de GPO Local | ✅ | ✅ |
| Interface em Português | ❌ | ✅ |
| Gerenciamento de GPO do AD | ❌ | ✅ |
| Verificação de permissões AD | ❌ | ✅ |
| Listagem de GPOs do domínio | ❌ | ✅ |
| Detalhes de GPO (versão, datas) | ❌ | ✅ |
| Controle de acesso (leitura/escrita) | ❌ | ✅ |

---

## 🆕 Novidades desta Versão

### 🌐 Gerenciamento de GPO do Active Directory
- **Listar todos os GPOs** do domínio via consulta LDAP
- **Visualizar e editar** políticas de qualquer GPO
- **Detalhes completos**: nome, GUID, versão, datas de criação/modificação, caminho SYSVOL
- **Verificação automática de permissões**: detecta se o usuário é Administrador de Domínio
- **Modo somente leitura** automático para usuários sem privilégios de escrita
- **Confirmação de segurança** antes de aplicar alterações em GPOs de domínio

### 🇧🇷 Interface 100% em Português Brasileiro
- Todos os menus, formulários, botões, mensagens e diálogos traduzidos
- Labels de status, colunas de tabelas e tooltips em PT-BR
- Mensagens de erro e sucesso contextualizadas

---

## 📸 Capturas de Tela

### Tela Principal
```
┌─────────────────────────────────────────────────────┐
│ Arquivo  Visualizar  Localizar  Compartilhar  Ajuda │
├──────────┬──────────────────────┬───────────────────┤
│ Categ.   │ Políticas            │ Título da política│
│ ├─Sistema│ ├─ Configuração A    │                   │
│ ├─Rede   │ ├─ Configuração B    │ Descrição da      │
│ └─...    │ └─ Configuração C    │ política          │
├──────────┴──────────────────────┴───────────────────┤
│ Origem do computador: GPO Local | Origem: GPO Local │
└─────────────────────────────────────────────────────┘
```

### Diálogo de GPO do Domínio (Ctrl+D)
```
┌─ Abrir GPO do Domínio - Active Directory ───────────┐
│ Domínio: [empresa.local        ] [Atualizar]        │
│                                                      │
│ Suas permissões: Administrador de Domínio            │
│ ✓ Você tem permissões de administrador.              │
│                                                      │
│ GPOs:                    │ Detalhes:                  │
│ ├─ Default Domain Policy │ Nome: Default Domain Pol.. │
│ ├─ Firewall Policy       │ GUID: {31B2F340-016D...}  │
│ ├─ Security Baseline     │ Versão: 15                 │
│ └─ Desktop Restrictions  │ Criado: 15/01/2020         │
│                          │ Escrita: Sim ✓             │
│ Total: 4 GPOs            │                            │
│                                                      │
│ Caminho: \\empresa.local\SYSVOL\...\Policies\{...}  │
│                              [OK]  [Cancelar]        │
└──────────────────────────────────────────────────────┘
```

---

## 📥 Instalação

### Opção 1: Download do executável
1. Vá em [**Releases**](https://github.com/ManoAlee/GPO-Plus/releases)
2. Baixe o `GPO.Plus.exe`
3. Execute como **Administrador**

### Opção 2: Compilar a partir do código-fonte
```bash
git clone https://github.com/ManoAlee/GPO-Plus.git
cd GPO-Plus/PolicyPlus
msbuild PolicyPlus.vbproj /p:Configuration=Release
```

O executável será gerado em `PolicyPlus\bin\Release\Policy Plus.exe`.

---

## 🚀 Como Usar

### Editar Políticas Locais
1. Execute o GPO Plus como **Administrador**
2. As políticas locais são carregadas automaticamente
3. Navegue pelas categorias no painel esquerdo
4. Clique duas vezes em uma política para editar
5. Salve com **Ctrl+S**

### Gerenciar GPOs do Active Directory
1. Pressione **Ctrl+D** ou vá em **Arquivo > Abrir GPO do Domínio**
2. O domínio é detectado automaticamente
3. Clique em **Atualizar** para listar os GPOs
4. Selecione um GPO e veja os detalhes
5. Clique em **OK** para abrir

### Atalhos de Teclado

| Atalho | Ação |
|--------|------|
| `Ctrl+D` | Abrir GPO do Domínio |
| `Ctrl+O` | Abrir Recursos de Política |
| `Ctrl+S` | Salvar Políticas |
| `Ctrl+F` | Localizar por Texto |
| `Ctrl+G` | Localizar por ID |
| `Ctrl+R` | Localizar por Registro |
| `F3` | Localizar Próximo |
| `Shift+F3` | Resultados da Pesquisa |

---

## ⚡ Funcionalidades

### Origens de Política Suportadas

| Origem | Descrição | Leitura | Escrita |
|--------|-----------|:-------:|:-------:|
| **GPO Local** | Política de grupo local da máquina | ✅ | ✅ |
| **GPO do Domínio** | GPOs do Active Directory via SYSVOL | ✅ | ✅* |
| **Registro Local** | Edição direta no Windows Registry | ✅ | ✅ |
| **Arquivo POL** | Arquivos de política individuais | ✅ | ✅ |
| **GPO de Usuário** | Políticas por usuário (SID) | ✅ | ✅ |
| **Hive de Usuário** | Arquivos NTUSER.DAT offline | ✅ | ✅ |
| **Espaço Temporário** | Área de testes descartável | ✅ | ✅ |

> \* Requer permissões de Administrador de Domínio ou delegação adequada.

### Importação e Exportação

- **POL** — Formato nativo de políticas do Windows
- **REG** — Arquivos de registro do Windows
- **SPOL** — Formato de Política Semântica (portável)

### Pesquisa Avançada

- **Por Texto** — Busca no título, descrição ou comentário
- **Por ID** — Localiza por identificador único da política
- **Por Registro** — Encontra pela chave do Registry afetada

### Administração de Templates

- Download automático de ADMX da Microsoft
- Suporte a múltiplos idiomas ADML
- Visualização de produtos e definições de suporte

---

## 🏗️ Arquitetura do Projeto

```
PolicyPlus/
├── PolicyPlus/
│   ├── My Project/              # Configurações do projeto VB.NET
│   │   ├── AssemblyInfo.vb
│   │   ├── Application.Designer.vb
│   │   ├── Resources.resx
│   │   └── Settings.settings
│   │
│   ├── # ── Núcleo ──────────────────────────────────
│   ├── AdmxFile.vb              # Parser de arquivos ADMX
│   ├── AdmlFile.vb              # Parser de arquivos ADML (idiomas)
│   ├── AdmxBundle.vb            # Gerenciador de workspace ADMX
│   ├── AdmxStructures.vb        # Estruturas de dados ADMX
│   ├── CompiledStructures.vb    # Estruturas compiladas
│   ├── PolicyProcessing.vb      # Processamento de políticas
│   ├── PolicySource.vb          # Interface IPolicySource + PolFile
│   ├── PolicyLoader.vb          # Carregador de origens de política
│   ├── PolicyStructures.vb      # Estruturas de políticas
│   ├── PresentationStructures.vb # Estruturas de apresentação
│   ├── ConfigurationStorage.vb  # Armazenamento de configurações
│   │
│   ├── # ── Active Directory (NOVO) ─────────────────
│   ├── AdGpoManager.vb          # Gerenciador de GPOs do AD
│   │
│   ├── # ── Formulários Principais ──────────────────
│   ├── Main.vb                  # Janela principal
│   ├── EditSetting.vb           # Editor de configuração de política
│   ├── OpenPol.vb               # Abrir recursos de política
│   ├── OpenDomainGpo.vb         # Abrir GPO do domínio (NOVO)
│   ├── OpenAdmxFolder.vb        # Abrir pasta ADMX
│   │
│   ├── # ── Pesquisa ────────────────────────────────
│   ├── FindById.vb              # Localizar por ID
│   ├── FindByText.vb            # Localizar por texto
│   ├── FindByRegistry.vb        # Localizar por registro
│   ├── FindResults.vb           # Resultados da pesquisa
│   ├── FilterOptions.vb         # Opções de filtro
│   │
│   ├── # ── Importação/Exportação ───────────────────
│   ├── ImportReg.vb             # Importar REG
│   ├── ExportReg.vb             # Exportar REG
│   ├── ImportSpol.vb            # Importar política semântica
│   ├── EditPol.vb               # Editor POL bruto
│   │
│   ├── # ── Detalhes e Inspeção ─────────────────────
│   ├── DetailAdmx.vb            # Detalhes ADMX
│   ├── DetailCategory.vb        # Detalhes de categoria
│   ├── DetailPolicy.vb          # Detalhes de política
│   ├── DetailProduct.vb         # Detalhes de produto
│   ├── DetailSupport.vb         # Detalhes de suporte
│   ├── InspectPolicyElements.vb # Inspetor de elementos
│   ├── InspectSpolFragment.vb   # Fragmento de política semântica
│   │
│   ├── # ── Utilitários ─────────────────────────────
│   ├── RegFile.vb               # Parser de arquivos REG
│   ├── SpolFile.vb              # Parser de arquivos SPOL
│   ├── CmtxFile.vb              # Parser de comentários CMTX
│   ├── PInvoke.vb               # Chamadas nativas Win32
│   ├── Privilege.vb             # Gerenciamento de privilégios
│   ├── SystemInfo.vb            # Informações do sistema
│   ├── BitReinterpretation.vb   # Reinterpretação de bits
│   ├── XmlExtensions.vb         # Extensões XML
│   │
│   ├── # ── Recursos ────────────────────────────────
│   ├── Main.pt-BR.resx          # Recursos em Português BR
│   ├── *.resx                   # Recursos dos formulários
│   └── PolicyPlus.vbproj        # Arquivo do projeto
│
├── docs/                        # Documentação
│   ├── Components.md            # Componentes do sistema
│   └── Lexicon.md               # Glossário técnico
│
├── .gitignore
├── ATTRIBUTION.md               # Atribuições e créditos
├── LICENSE                      # Licença MIT
└── README.md                    # Este arquivo
```

---

## 💻 Requisitos do Sistema

### Mínimos
- **SO:** Windows Vista SP2 ou superior
- **.NET Framework:** 4.5.2 ou superior (pré-instalado no Windows 10+)
- **RAM:** 256 MB disponíveis
- **Disco:** 10 MB

### Para Gerenciamento de GPO do AD
- Máquina associada a um domínio Active Directory
- Acesso de rede ao controlador de domínio (porta LDAP 389)
- Acesso ao compartilhamento SYSVOL (`\\dominio\SYSVOL`)
- **Para edição:** Membro de Domain Admins, Enterprise Admins ou Group Policy Creator Owners

---

## 🔨 Compilação

### Pré-requisitos
- Visual Studio 2015+ ou MSBuild Tools
- .NET Framework 4.5.2 SDK

### Via linha de comando
```powershell
# Clonar o repositório
git clone https://github.com/ManoAlee/GPO-Plus.git
cd GPO-Plus\PolicyPlus

# Compilar em modo Debug
msbuild PolicyPlus.vbproj /p:Configuration=Debug

# Compilar em modo Release
msbuild PolicyPlus.vbproj /p:Configuration=Release
```

### Via Visual Studio
1. Abra `PolicyPlus\PolicyPlus.sln`
2. Selecione a configuração **Release**
3. Compile com **Ctrl+Shift+B**

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um **Fork** do repositório
2. Crie uma **branch** para sua feature (`git checkout -b feature/minha-feature`)
3. Faça o **commit** das alterações (`git commit -m 'Adiciona minha feature'`)
4. Faça o **push** para a branch (`git push origin feature/minha-feature`)
5. Abra um **Pull Request**

### Diretrizes
- Mantenha todas as strings de interface em **Português Brasileiro**
- Siga o padrão de código VB.NET existente
- Teste em máquinas com e sem domínio AD
- Documente novas funcionalidades

---

## 📜 Créditos e Referências

### Projeto Original
Este projeto é um fork de **[Policy Plus](https://github.com/Fleex255/PolicyPlus)** criado por **[Ben Nordick (Fleex255)](https://github.com/Fleex255)**.

> *"Policy Plus is intended to make the power of Group Policy settings available to everyone."*
> — Ben Nordick

### Modificações neste Fork
Desenvolvido por **[Alessandro Meneses (ManoAlee)](https://github.com/ManoAlee)**:
- Tradução completa para Português Brasileiro
- Módulo de gerenciamento de GPO do Active Directory
- Verificação de permissões de domínio
- Melhorias na interface e usabilidade

### Ícones
Todos os ícones são do conjunto [FamFamFam "Silk"](http://www.famfamfam.com/lab/icons/silk/), disponível sob Creative Commons Attribution 2.5.

### Documentação de Referência
- [Group Policy File Formats (Microsoft)](https://msdn.microsoft.com/en-us/library/aa374150(v=vs.85).aspx)
- [Group Policy ADMX Syntax Reference Guide](https://technet.microsoft.com/en-us/library/cc753471(v=ws.10).aspx)
- [System.DirectoryServices Namespace](https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices)

---

## 📄 Licença

Este projeto está licenciado sob a **Licença MIT** — veja o arquivo [LICENSE](LICENSE) para detalhes.

```
MIT License

Copyright (c) 2016-2021 Ben Nordick (Projeto Original)
Copyright (c) 2025-2026 Alessandro Meneses (Modificações PT-BR e AD)
```

---

<p align="center">
  <b>⭐ Se este projeto foi útil, considere dar uma estrela no repositório!</b>
</p>

<p align="center">
  <a href="https://github.com/Fleex255/PolicyPlus">📦 Projeto Original</a> •
  <a href="https://github.com/ManoAlee/GPO-Plus/issues">🐛 Reportar Bug</a> •
  <a href="https://github.com/ManoAlee/GPO-Plus/issues">💡 Sugerir Feature</a>
</p>
