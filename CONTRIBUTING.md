# Contribuindo para o GPO Plus

Obrigado por considerar contribuir com o GPO Plus! Este documento descreve o fluxo recomendado para enviar correções e novas funcionalidades.

## Como contribuir

1. Fork o repositório para sua conta GitHub
2. Clone seu fork localmente
   ```bash
   git clone https://github.com/<seu-usuario>/GPO-Plus.git
   cd GPO-Plus/PolicyPlus
   ```
3. Crie uma branch para sua alteração
   ```bash
   git checkout -b feature/minha-feature
   ```
4. Faça alterações pequenas e testáveis. Siga o padrão de código VB.NET já existente.
5. Adicione testes manuais e verifique que a aplicação compila em `Release`.
6. Faça commit das alterações com mensagem descritiva
   ```bash
   git add -A
   git commit -m "Descrição curta: o que foi modificado"
   ```
7. Faça push para seu fork e abra um Pull Request contra `ManoAlee/GPO-Plus:master`

## Diretrizes de código
- Strings de interface UI devem ser mantidas em `Main.pt-BR.resx` (ou no `.resx` do formulário) para facilitar tradução.
- Use 4 espaços para identação em arquivos `.vb`.
- Mantenha métodos pequenos e coesos; extraia lógica complexa para classes utilitárias quando necessário.

## Busca de issues e revisão
- Antes de trabalhar em uma feature, abra uma *issue* descrevendo a intenção.
- PRs grandes podem ser divididos em etapas menores para revisão incremental.
- Inclua descrição do teste manual executado e, se possível, prints ou logs relevantes.

## Testes
- Teste em máquinas com domínio AD e sem domínio.
- Verifique cenários de permissões (usuário padrão, Administrador local, Domain Admin).

---

Agradecemos pela sua contribuição! :)
