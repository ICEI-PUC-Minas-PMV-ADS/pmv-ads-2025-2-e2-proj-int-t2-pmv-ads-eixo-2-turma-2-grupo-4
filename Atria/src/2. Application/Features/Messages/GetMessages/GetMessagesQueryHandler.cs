using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Atria.Application.Features.Messages.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, IEnumerable<MessageDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMessagesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context.MensagensPrivadas
            .AsNoTracking()
            .Where(m => m.FkRemetente == request.UserId || m.FkDestinatario == request.UserId)
            .OrderByDescending(m => m.DataEnvio)
            .Take(100)
            .Select(m => new MessageDto
            {
                Id = m.IdMensagem,
                Content = m.Conteudo,
                SentAt = m.DataEnvio,
                Read = m.Lida,
                SenderId = m.FkRemetente,
                RecipientId = m.FkDestinatario
            })
            .ToListAsync(cancellationToken);

        return messages;
    }
}